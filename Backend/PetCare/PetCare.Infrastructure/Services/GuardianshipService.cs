namespace PetCare.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Infrastructure.Persistence;

/// <summary>
/// Provides operations related to guardianship management within the application.
/// </summary>
/// <remarks>This service defines the contract for guardianship-related functionality. Implementations of this
/// service should encapsulate business logic for managing guardianship entities and related actions. Consumers should
/// use this service to interact with guardianship features rather than accessing data or logic directly.</remarks>
public class GuardianshipService : IGuardianshipService
{
    private readonly IGuardianshipRepository guardianships;
    private readonly IAnimalRepository animals;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuardianshipService"/> class with the specified repositories and database.
    /// context.
    /// </summary>
    /// <param name="guardianships">The repository used to manage guardianship records. Cannot be null.</param>
    /// <param name="animals">The repository used to access animal data. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="guardianships"/>, <paramref name="animals"/>, or <paramref name="db"/> is null.</exception>
    public GuardianshipService(
        IGuardianshipRepository guardianships,
        IAnimalRepository animals)
    {
        this.guardianships = guardianships ?? throw new ArgumentNullException(nameof(guardianships));
        this.animals = animals ?? throw new ArgumentNullException(nameof(animals));
    }

    /// <summary>
    /// Creates a new guardianship for the specified user and animal, with an optional grace period before the
    /// guardianship becomes active.
    /// </summary>
    /// <remarks>This method ensures that only one active guardianship exists per user and animal combination.
    /// If a guardianship is already active, the method will not create a new one and will throw an exception.</remarks>
    /// <param name="userId">The unique identifier of the user who will be assigned as the guardian.</param>
    /// <param name="animalId">The unique identifier of the animal for which the guardianship is being created.</param>
    /// <param name="graceDays">The number of days to delay the activation of the guardianship. Defaults to 3 days.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Guardianship"/> instance representing the newly created guardianship.</returns>
    /// <exception cref="InvalidOperationException">Thrown if an active guardianship already exists for the specified user and animal.</exception>
    public async Task<Guardianship> CreateAsync(Guid userId, Guid animalId, int graceDays = 3, CancellationToken cancellationToken = default)
    {
        var exists = await this.guardianships.ExistsActiveByUserAndAnimalAsync(userId, animalId, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException("Активна опіка для цієї тварини вже існує.");
        }

        var g = Guardianship.Create(userId, animalId, TimeSpan.FromDays(graceDays));
        await this.guardianships.AddAsync(g, cancellationToken);
        return g;
    }

    /// <summary>
    /// Activates a guardianship and marks the associated animal as under care after recording the first payment.
    /// </summary>
    /// <param name="guardianshipId">The unique identifier of the guardianship to activate. Must correspond to an existing guardianship.</param>
    /// <param name="donationId">The unique identifier of the donation representing the first payment. Must correspond to an existing donation.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
    /// <returns>A task that represents the asynchronous activation operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the specified guardianship or its associated animal cannot be found.</exception>
    public async Task ActivateWithFirstPaymentAsync(Guid guardianshipId, Guid donationId, CancellationToken cancellationToken = default)
    {
        var g = await this.guardianships.GetByIdForUpdateAsync(guardianshipId, cancellationToken)
            ?? throw new InvalidOperationException("Опіку не знайдено.");

        g.AddDonation(donationId);
        g.Activate();

        var animal = await this.animals.GetByIdAsync(g.AnimalId, cancellationToken)
            ?? throw new InvalidOperationException("Тварину не знайдено.");
        animal.MarkAsUnderCare();

        await this.animals.UpdateAsync(animal, cancellationToken);
        await this.guardianships.UpdateAsync(g, cancellationToken);
    }

    /// <summary>
    /// Initiates a payment requirement for the specified guardianship, allowing a grace period before payment is due.
    /// </summary>
    /// <remarks>No payment requirement is set if the guardianship has already been completed.</remarks>
    /// <param name="guardianshipId">The unique identifier of the guardianship for which payment is to be required.</param>
    /// <param name="graceDays">The number of days to allow as a grace period before the payment is required. Must be non-negative. Defaults to
    /// 3 days.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if this token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task completes when the payment requirement has been set
    /// or if the guardianship is already completed.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a guardianship with the specified identifier does not exist.</exception>
    public async Task RequirePaymentAsync(Guid guardianshipId, int graceDays = 3, CancellationToken cancellationToken = default)
    {
        var g = await this.guardianships.GetByIdForUpdateAsync(guardianshipId, cancellationToken)
            ?? throw new InvalidOperationException("Опіку не знайдено.");

        if (g.Status == GuardianshipStatus.Completed)
        {
            return;
        }

        g.RequirePayment(TimeSpan.FromDays(graceDays));
        await this.guardianships.UpdateAsync(g, cancellationToken);
    }

    /// <summary>
    /// Marks the specified guardianship as complete and updates related animal and subscription records as needed.
    /// </summary>
    /// <remarks>If the guardianship is associated with an animal, the animal's care status is updated
    /// accordingly. If <paramref name="cancelSubscription"/> is <see langword="true"/>, any related payment
    /// subscription is canceled.</remarks>
    /// <param name="guardianshipId">The unique identifier of the guardianship to complete.</param>
    /// <param name="cancelSubscription">Indicates whether to cancel the associated payment subscription. Set to <see langword="true"/> to cancel the
    /// subscription; otherwise, <see langword="false"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a guardianship with the specified <paramref name="guardianshipId"/> does not exist.</exception>
    public async Task CompleteAsync(Guid guardianshipId, bool cancelSubscription = false, CancellationToken cancellationToken = default)
    {
        var g = await this.guardianships.GetByIdForUpdateAsync(guardianshipId, cancellationToken)
            ?? throw new InvalidOperationException("Опіку не знайдено.");

        g.Complete();

        var animal = await this.animals.GetByIdAsync(g.AnimalId, cancellationToken);
        if (animal is not null)
        {
            animal.MarkAsNotUnderCare();
            await this.animals.UpdateAsync(animal, cancellationToken);
        }

        await this.guardianships.UpdateAsync(g, cancellationToken);

        if (cancelSubscription)
        {
            await this.guardianships.CancelSubscriptionAsync(guardianshipId, cancellationToken);
        }
    }

    /// <summary>
    /// Automatically completes all expired guardianships that require payment as of the specified UTC time.
    /// </summary>
    /// <remarks>This method updates the status of expired guardianships and marks associated animals as not
    /// under care. The operation is performed asynchronously and may involve multiple data updates.</remarks>
    /// <param name="utcNow">The current UTC date and time used to determine which guardianships have expired and require completion.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>The number of guardianships that were automatically completed.</returns>
    public async Task<int> AutoCompleteExpiredAsync(DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var toComplete = await this.guardianships.ListExpiredRequiresPaymentAsync(utcNow, cancellationToken);
        foreach (var item in toComplete)
        {
            var tracked = await this.guardianships.GetByIdForUpdateAsync(item.Id, cancellationToken);
            if (tracked is null)
            {
                continue;
            }

            tracked.Complete();

            var animal = await this.animals.GetByIdAsync(tracked.AnimalId, cancellationToken);
            if (animal is not null)
            {
                animal.MarkAsNotUnderCare();
                await this.animals.UpdateAsync(animal, cancellationToken);
            }

            await this.guardianships.UpdateAsync(tracked, cancellationToken);
        }

        return toComplete.Count;
    }

    /// <summary>
    /// Asynchronously retrieves a guardianship entity by its unique identifier, including related details.
    /// </summary>
    /// <param name="id">The unique identifier of the guardianship to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the guardianship entity if found;
    /// otherwise, <see langword="null"/>.</returns>
    public Task<Guardianship?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => this.guardianships.GetByIdWithDetailsAsync(id, cancellationToken);

    /// <summary>
    /// Retrieves a read-only list of guardianship records associated with the specified user, optionally filtered by
    /// guardianship status.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose guardianship records are to be retrieved.</param>
    /// <param name="status">An optional status value to filter the guardianship records. If null, records of all statuses are returned.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of guardianship
    /// records for the specified user. The list will be empty if no records are found.</returns>
    public Task<IReadOnlyList<Guardianship>> GetByUserAsync(Guid userId, GuardianshipStatus? status = null, CancellationToken cancellationToken = default)
        => this.guardianships.ListByUserAsync(userId, status, cancellationToken);

    /// <summary>
    /// Asynchronously retrieves a read-only list of guardianship records associated with the specified animal,
    /// optionally filtered by guardianship status.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal for which guardianship records are to be retrieved.</param>
    /// <param name="status">An optional status value to filter the guardianship records. If null, all statuses are included.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of guardianship
    /// records matching the specified criteria. The list will be empty if no records are found.</returns>
    public Task<IReadOnlyList<Guardianship>> GetByAnimalAsync(Guid animalId, GuardianshipStatus? status = null, CancellationToken cancellationToken = default)
        => this.guardianships.ListByAnimalAsync(animalId, status, cancellationToken);
}

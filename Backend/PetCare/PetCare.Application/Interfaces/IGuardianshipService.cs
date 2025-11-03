namespace PetCare.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;

/// <summary>
/// Defines operations for managing guardianships (user ↔ animal care).
/// </summary>
public interface IGuardianshipService
{
    /// <summary>
    /// Asynchronously creates a new guardianship relationship between a user and an animal.
    /// </summary>
    /// <param name="userId">The unique identifier of the user who will become the guardian.</param>
    /// <param name="animalId">The unique identifier of the animal for which the guardianship is being created.</param>
    /// <param name="graceDays">The number of grace days, starting from the creation date, during which the guardianship can be canceled. Must
    /// be zero or greater. The default is 3.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created Guardianship instance.</returns>
    Task<Guardianship> CreateAsync(Guid userId, Guid animalId, int graceDays = 3, CancellationToken cancellationToken = default);

    /// <summary>
    /// Activates a guardianship using the specified first payment donation.
    /// </summary>
    /// <param name="guardianshipId">The unique identifier of the guardianship to activate.</param>
    /// <param name="donationId">The unique identifier of the donation representing the first payment.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous activation operation.</returns>
    Task ActivateWithFirstPaymentAsync(Guid guardianshipId, Guid donationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initiates a payment requirement for the specified guardianship, allowing an optional grace period before
    /// enforcement.
    /// </summary>
    /// <param name="guardianshipId">The unique identifier of the guardianship for which payment is required.</param>
    /// <param name="graceDays">The number of days to allow as a grace period before enforcing the payment requirement. Must be zero or greater.
    /// Defaults to 3.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RequirePaymentAsync(Guid guardianshipId, int graceDays = 3, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks the specified guardianship as complete asynchronously, with an option to cancel any associated
    /// subscription.
    /// </summary>
    /// <param name="guardianshipId">The unique identifier of the guardianship to complete.</param>
    /// <param name="cancelSubscription">true to cancel the associated subscription when completing the guardianship; otherwise, false.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous completion operation.</returns>
    Task CompleteAsync(Guid guardianshipId, bool cancelSubscription = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks all expired items as completed as of the specified UTC time asynchronously.
    /// </summary>
    /// <param name="utcNow">The current UTC date and time used to determine which items are considered expired.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of items that were marked
    /// as completed.</returns>
    Task<int> AutoCompleteExpiredAsync(DateTime utcNow, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a guardianship entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the guardianship to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the guardianship entity if found;
    /// otherwise, null.</returns>
    Task<Guardianship?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all guardianship records associated with the specified user, optionally filtered by
    /// guardianship status.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose guardianship records are to be retrieved.</param>
    /// <param name="status">An optional status value to filter the guardianship records. If null, records with any status are returned.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of guardianship
    /// records matching the specified criteria. The list is empty if no records are found.</returns>
    Task<IReadOnlyList<Guardianship>> GetByUserAsync(Guid userId, GuardianshipStatus? status = null, CancellationToken cancellationToken = default);

    /// <summary>s
    /// Asynchronously retrieves all guardianship records associated with the specified animal, optionally filtered by
    /// guardianship status.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal for which to retrieve guardianship records.</param>
    /// <param name="status">An optional status value to filter the guardianship records. If null, records with any status are returned.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of guardianship
    /// records matching the specified criteria. The list is empty if no records are found.</returns>
    Task<IReadOnlyList<Guardianship>> GetByAnimalAsync(Guid animalId, GuardianshipStatus? status = null, CancellationToken cancellationToken = default);
}

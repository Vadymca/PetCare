namespace PetCare.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Specifications.Shelter;
using PetCare.Domain.ValueObjects;
using PetCare.Infrastructure.Persistence;

/// <summary>
/// Repository implementation for managing <see cref="Shelter"/> entities.
/// </summary>
public class ShelterRepository : GenericRepository<Shelter>, IShelterRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShelterRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ShelterRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<Shelter?> GetShelterByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken = default)
        => await this.Context.Set<Shelter>()
            .Include(s => s.IoTDevices)
            .Where(new ShelterByDeviceSpecification(deviceId).ToExpression())
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Shelter>> GetByManagerIdAsync(Guid managerId, CancellationToken cancellationToken = default)
        => await this.FindAsync(new SheltersByManagerSpecification(managerId), cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Shelter>> GetWithFreeCapacityAsync(CancellationToken cancellationToken = default)
        => await this.FindAsync(new SheltersWithFreeCapacitySpecification(), cancellationToken);

    /// <inheritdoc />
    public async Task<Shelter?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Slug не може бути порожнім.", nameof(slug));
        }

        var shelter = await this.Context.Set<Shelter>()
            .AsNoTracking()
            .Include(s => s.Animals)
            .Include(s => s.Donations)
            .Include(s => s.VolunteerTasks)
            .Include(s => s.AnimalAidRequests)
            .Include(s => s.IoTDevices)
            .Include(s => s.Events)
            .Include(s => s.Subscribers)
            .FirstOrDefaultAsync(s => s.Slug == Slug.FromExisting(slug), cancellationToken);

        return shelter ?? throw new InvalidOperationException($"Притулок зі slug '{slug}' не знайдено.");
    }

    /// <summary>
    /// Gets a paginated list of shelters ordered by creation date (newest first).
    /// </summary>
    /// <param name="page">The current page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A tuple containing the shelters and total count.</returns>
    public async Task<(IReadOnlyList<Shelter> Shelters, int TotalCount)> GetSheltersAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = this.Context.Set<Shelter>()
            .AsNoTracking()
            .Include(s => s.Manager)
            .Include(s => s.Animals)
            .OrderByDescending(s => s.CreatedAt);

        var total = await query.CountAsync(cancellationToken);

        var shelters = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (shelters, total);
    }

    /// <summary>
    /// Gets a shelter by its unique identifier, including related entities.
    /// </summary>
    /// <param name="id">The unique identifier of the shelter.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The shelter with the specified identifier.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no shelter is found with the specified ID.</exception>
    public new async Task<Shelter?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var shelter = await this.Context.Set<Shelter>()
            .AsNoTracking()
            .Include(s => s.Animals)
            .Include(s => s.VolunteerTasks)
            .Include(s => s.Events)
            .Include(s => s.Donations)
            .Include(s => s.Manager)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return shelter ?? throw new InvalidOperationException($"Притулок з Id '{id}' не знайдено.");
    }

    /// <summary>
    /// Asynchronously adds a new shelter to the data store and retrieves the fully populated shelter entity, including related entities.
    /// </summary>
    /// <param name="shelter">The shelter to add. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>The added shelter entity with related data populated.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the shelter parameter is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the shelter cannot be found in the data store after being added.</exception>
    public async Task<Shelter> AddShelterAsync(Shelter shelter, CancellationToken cancellationToken = default)
    {
        if (shelter == null)
        {
            throw new ArgumentNullException(nameof(shelter), "Притулок не може бути null.");
        }

        await this.AddAsync(shelter, cancellationToken);

        var fullShelter = await this.Context.Shelters
            .Include(s => s.Manager)
            .FirstOrDefaultAsync(s => s.Id == shelter.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Притулок з Id '{shelter.Id}' не знайдено після додавання.");

        return fullShelter;
    }

   /// <summary>
   /// Subscribes a user to the specified shelter asynchronously.
   /// </summary>
   /// <param name="shelterId">The unique identifier of the shelter to which the user will be subscribed.</param>
   /// <param name="userId">The unique identifier of the user to subscribe to the shelter.</param>
   /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
   /// <returns>A task that represents the asynchronous operation. The task result contains the created shelter subscription.</returns>
   /// <exception cref="InvalidOperationException">Thrown if a shelter with the specified shelterId does not exist.</exception>
    public async Task<ShelterSubscription> SubscribeUserAsync(Guid shelterId, Guid userId, CancellationToken cancellationToken = default)
    {
        var shelter = await this.Context.Shelters
            .Include(s => s.Subscribers)
            .FirstOrDefaultAsync(s => s.Id == shelterId, cancellationToken)
            ?? throw new InvalidOperationException($"Притулок з Id '{shelterId}' не знайдено.");

        var subscription = shelter.SubscribeUser(userId);

        this.Context.Set<ShelterSubscription>().Add(subscription);
        await this.Context.SaveChangesAsync(cancellationToken);

        return subscription;
    }

    /// <summary>
    /// Asynchronously removes a user's subscription from the specified shelter.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter from which the user will be unsubscribed.</param>
    /// <param name="userId">The unique identifier of the user to unsubscribe from the shelter.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the unsubscribe operation.</param>
    /// <returns>A task that represents the asynchronous unsubscribe operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a shelter with the specified <paramref name="shelterId"/> does not exist.</exception>
    public async Task UnsubscribeUserAsync(Guid shelterId, Guid userId, CancellationToken cancellationToken = default)
    {
        var shelter = await this.Context.Shelters
            .Include(s => s.Subscribers)
            .FirstOrDefaultAsync(s => s.Id == shelterId, cancellationToken)
            ?? throw new InvalidOperationException($"Притулок з Id '{shelterId}' не знайдено.");

        var subscription = shelter.UnsubscribeUser(userId);

        if (subscription != null)
        {
            this.Context.Set<ShelterSubscription>().Remove(subscription);
        }

        await this.Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Attempts to increment the current occupancy count for the specified shelter if capacity allows.
    /// </summary>
    /// <remarks>If the shelter's current occupancy is equal to or greater than its capacity, the operation
    /// will not succeed and an exception will be thrown. The shelter's last updated timestamp is also set to the
    /// current UTC time upon a successful increment.</remarks>
    /// <param name="shelterId">The unique identifier of the shelter whose occupancy is to be incremented.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the shelter is full or does not exist.</exception>
    public async Task IncrementOccupancyAsync(Guid shelterId, CancellationToken cancellationToken = default)
    {
        var updated = await this.Context.Shelters
            .Where(s => s.Id == shelterId && s.CurrentOccupancy < s.Capacity)
            .ExecuteUpdateAsync(
            s => s
                .SetProperty(x => x.CurrentOccupancy, x => x.CurrentOccupancy + 1)
                .SetProperty(x => x.UpdatedAt, x => DateTime.UtcNow),
            cancellationToken);

        if (updated == 0)
        {
            throw new InvalidOperationException("Притулок заповнений або не знайдено.");
        }
    }

    /// <summary>
    /// Decrements the current occupancy count for the specified shelter if it is greater than zero.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter whose occupancy is to be decremented.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the specified shelter does not exist or its current occupancy is already zero.</exception>
    public async Task DecrementOccupancyAsync(Guid shelterId, CancellationToken cancellationToken = default)
    {
        var updated = await this.Context.Shelters
            .Where(s => s.Id == shelterId && s.CurrentOccupancy > 0)
            .ExecuteUpdateAsync(
            s => s
                .SetProperty(x => x.CurrentOccupancy, x => x.CurrentOccupancy - 1)
                .SetProperty(x => x.UpdatedAt, x => DateTime.UtcNow),
            cancellationToken);

        if (updated == 0)
        {
            throw new InvalidOperationException("Притулок не знайдено або зайнятість вже дорівнює нулю.");
        }
    }
}

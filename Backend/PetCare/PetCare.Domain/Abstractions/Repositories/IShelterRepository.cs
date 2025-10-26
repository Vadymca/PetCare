namespace PetCare.Domain.Abstractions.Repositories;

using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Repository interface for accessing shelter entities.
/// </summary>
public interface IShelterRepository : IRepository<Shelter>
{
    /// <summary>
    /// Retrieves a shelter by its unique slug.
    /// </summary>
    /// <param name="slug">The slug of the shelter.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the shelter if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Shelter?> GetBySlugAsync(
        string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a shelter that already contains a specific IoT device.
    /// </summary>
    /// <param name="deviceId">The ID of the IoT device.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the shelter if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Shelter?> GetShelterByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all shelters managed by a specific user.
    /// </summary>
    /// <param name="managerId">The unique identifier of the manager.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of shelters.</returns>
    Task<IReadOnlyList<Shelter>> GetByManagerIdAsync(Guid managerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves shelters with available capacity.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of shelters with free capacity.</returns>
    Task<IReadOnlyList<Shelter>> GetWithFreeCapacityAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a paginated list of shelters along with the total number of shelters available.
    /// </summary>
    /// <remarks>The returned list may contain fewer items than the specified page size if there are not
    /// enough shelters remaining. This method does not guarantee thread safety; callers should ensure appropriate
    /// synchronization if accessing from multiple threads.</remarks>
    /// <param name="page">The zero-based page index indicating which page of results to retrieve. Must be greater than or equal to 0.</param>
    /// <param name="pageSize">The maximum number of shelters to include in the returned page. Must be greater than 0.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The result contains a read-only list of shelters for the
    /// specified page and the total count of shelters available.</returns>
    Task<(IReadOnlyList<Shelter> Shelters, int TotalCount)> GetSheltersAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a new shelter to the system.
    /// </summary>
    /// <param name="shelter">The shelter entity to add. Cannot be null. All required properties of the shelter must be set before calling
    /// this method.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added shelter entity, including
    /// any system-assigned properties such as its unique identifier.</returns>
    Task<Shelter> AddShelterAsync(Shelter shelter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Subscribes a user to notifications or updates for the specified shelter asynchronously.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter to which the user will be subscribed.</param>
    /// <param name="userId">The unique identifier of the user to subscribe.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a ShelterSubscription object
    /// representing the user's subscription to the shelter.</returns>
    Task<ShelterSubscription> SubscribeUserAsync(Guid shelterId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously unsubscribes the specified user from notifications or updates related to the given shelter.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter from which the user will be unsubscribed.</param>
    /// <param name="userId">The unique identifier of the user to unsubscribe.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the unsubscribe operation.</param>
    /// <returns>A task that represents the asynchronous unsubscribe operation.</returns>
    Task UnsubscribeUserAsync(Guid shelterId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously increments the occupancy count for the specified shelter.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter whose occupancy count will be incremented.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task IncrementOccupancyAsync(Guid shelterId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Decrements the recorded occupancy count for the specified shelter asynchronously.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter whose occupancy count will be decremented.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DecrementOccupancyAsync(Guid shelterId, CancellationToken cancellationToken = default);
}

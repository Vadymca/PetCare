namespace PetCare.Domain.Abstractions.Repositories;

using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

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

    // ________________________________________AnimalAidRequest________________________________________________

    /// <summary>
    /// Asynchronously retrieves all animal aid requests from the database, including related donations, user, and
    /// shelter information.
    /// </summary>
    /// <remarks>The returned list includes all animal aid requests currently stored in the database. Related
    /// entities are loaded eagerly to provide complete information for each request.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all animal aid requests
    /// with their associated donations, user, and shelter data.</returns>
    Task<List<AnimalAidRequest>> GetAllAnimalAidRequestsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves an animal aid request by its unique identifier.
    /// </summary>
    /// <remarks>The returned <see cref="AnimalAidRequest"/> includes related donations, user, and shelter
    /// information. If no request with the specified identifier exists, the result is <see langword="null"/>.</remarks>
    /// <param name="id">The unique identifier of the animal aid request to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="AnimalAidRequest"/>
    /// if found; otherwise, <see langword="null"/>.</returns>
    Task<AnimalAidRequest> GetAnimalAidRequestByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves an animal aid request by its unique slug identifier.
    /// </summary>
    /// <remarks>The returned request includes related donations, user, and shelter information. The query is
    /// performed without tracking changes to the entities.</remarks>
    /// <param name="slug">The slug that uniquely identifies the animal aid request. Cannot be null, empty, or consist only of whitespace.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>The animal aid request that matches the specified slug.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="slug"/> is null, empty, or consists only of whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown if no animal aid request with the specified slug is found.</exception>
    Task<AnimalAidRequest> GetAnimalAidRequestBySlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new animal aid request and saves it to the data store asynchronously.
    /// </summary>
    /// <param name="request">The animal aid request to be created. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>The created <see cref="AnimalAidRequest"/> instance after it has been saved to the data store.</returns>
    Task<AnimalAidRequest> CreateAnimalAidRequestAsync(AnimalAidRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates an existing animal aid request in the data store.
    /// </summary>
    /// <remarks>The update is persisted to the underlying data store when the operation completes. If the
    /// specified request does not exist, no changes are made.</remarks>
    /// <param name="request">The animal aid request entity to update. Must not be null and should represent an existing request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateAnimalAidRequestAsync(AnimalAidRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the status of an existing animal aid request identified by the specified ID.
    /// </summary>
    /// <remarks>If no animal aid request with the specified ID exists, the method completes without making
    /// any changes.</remarks>
    /// <param name="id">The unique identifier of the animal aid request to update.</param>
    /// <param name="status">The new status to assign to the animal aid request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task completes when the status has been updated or
    /// if the request does not exist.</returns>
    Task UpdateAnimalAidRequestStatusAsync(Guid id, AidStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes the animal aid request identified by the specified ID, if it exists.
    /// </summary>
    /// <remarks>If no animal aid request with the specified ID exists, the method completes without
    /// performing any action. This method does not throw an exception if the request is not found.</remarks>
    /// <param name="id">The unique identifier of the animal aid request to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAnimalAidRequestAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a list of urgent animal aid requests, ordered by creation date in descending order.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of urgent animal aid
    /// requests, with the most recently created requests first. If no urgent requests exist, the list will be empty.</returns>
    Task<List<AnimalAidRequest>> GetUrgentAnimalAidRequestsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sums the total amount of completed donations for a specific animal aid request.
    /// </summary>
    /// <param name="aidRequestId">The unique identifier of the animal aid request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<decimal> SumCompletedByAidRequestIdAsync(
    Guid aidRequestId,
    CancellationToken cancellationToken = default);
}

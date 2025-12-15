namespace PetCare.Application.Interfaces;

using PetCare.Domain.Entities;
using PetCare.Domain.Enums;

/// <summary>
/// Defines the contract for services that manage animal aid requests.
/// </summary>
/// <remarks>Implementations of this interface provide operations related to creating, retrieving, or managing
/// requests for animal aid. The specific methods and behaviors are defined by the implementing class.</remarks>
public interface IAnimalAidRequestService
{
    /// <summary>
    /// Asynchronously retrieves all animal aid requests.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all animal aid requests.
    /// The list will be empty if no requests are found.</returns>
    Task<List<AnimalAidRequest>> GetAllAnimalAidRequestsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves an animal aid request by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the animal aid request to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the animal aid request associated
    /// with the specified identifier, or null if no matching request is found.</returns>
    Task<AnimalAidRequest> GetAnimalAidRequestByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves an animal aid request identified by the specified slug.
    /// </summary>
    /// <param name="slug">The unique slug that identifies the animal aid request to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="AnimalAidRequest"/>
    /// associated with the specified slug, or <see langword="null"/> if no matching request is found.</returns>
    Task<AnimalAidRequest> GetAnimalAidRequestBySlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously creates a new animal aid request.
    /// </summary>
    /// <param name="request">The animal aid request to create. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created animal aid request.</returns>
    Task<AnimalAidRequest> CreateAnimalAidRequestAsync(AnimalAidRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing animal aid request in the data store.
    /// </summary>
    /// <param name="request">The <see cref="AnimalAidRequest"/> entity to update.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous update operation.</returns>
    Task UpdateAnimalAidRequestAsync(AnimalAidRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the status of an existing animal aid request identified by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the animal aid request.</param>
    /// <param name="status">The new status to set.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous status update operation.</returns>
    Task UpdateAnimalAidRequestStatusAsync(Guid id, AidStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing animal aid request identified by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the animal aid request to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteAnimalAidRequestAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all urgent animal aid requests, ordered by creation date descending.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The result contains a list of urgent <see cref="AnimalAidRequest"/> entities.</returns>
    Task<List<AnimalAidRequest>> GetUrgentAnimalAidRequestsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Attaches a donation to an AnimalAidRequest.
    /// </summary>
    /// <param name="aidRequestId">ID of the AnimalAidRequest.</param>
    /// <param name="donationId">ID of the Donation to attach.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AttachDonationAsync(Guid aidRequestId, Guid donationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total collected amount for a specific animal aid request.
    /// </summary>
    /// <param name="aidRequestId"> Id of the AnimalAidRequest.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<decimal> GetCollectedAmountAsync(
    Guid aidRequestId,
    CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the number of donations for a given animal aid request.
    /// </summary>
    /// <param name="aidRequestId">The ID of the animal aid request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The count of donations.</returns>
    Task<int> GetDonationsCountAsync(Guid aidRequestId, CancellationToken cancellationToken = default);
}

namespace PetCare.Domain.Abstractions.Repositories;

using PetCare.Domain.Aggregates;

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
}

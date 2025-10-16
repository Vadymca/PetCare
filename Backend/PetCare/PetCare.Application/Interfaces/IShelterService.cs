namespace PetCare.Application.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Provides methods for retrieving shelter information with support for pagination.
/// </summary>
public interface IShelterService
{
    /// <summary>
    /// Asynchronously retrieves a paginated list of shelters along with the total number of shelters available.
    /// </summary>
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
    /// Asynchronously retrieves a shelter by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the shelter to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Shelter"/> if found;
    /// otherwise, <see langword="null"/>.</returns>
    Task<Shelter?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a shelter by its unique slug identifier.
    /// </summary>
    /// <param name="slug">The unique slug that identifies the shelter to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see cref="Shelter"/>
    /// if found; otherwise, <see langword="null"/>.</returns>
    Task<Shelter?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously creates a new shelter with the specified details and adds it to the repository.
    /// </summary>
    /// <remarks>If <paramref name="managerId"/> is provided, the specified user will be assigned the
    /// 'ShelterManager' role. The method validates input parameters and may throw exceptions for invalid data. This
    /// operation is asynchronous and may be cancelled via the provided cancellation token.</remarks>
    /// <param name="name">The name of the shelter. Cannot be null or empty.</param>
    /// <param name="address">The physical address of the shelter. Cannot be null or empty.</param>
    /// <param name="latitude">The latitude of the shelter's location in decimal degrees (-90 to 90).</param>
    /// <param name="longitude">The longitude of the shelter's location in decimal degrees (-180 to 180).</param>
    /// <param name="contactPhone">The contact phone number for the shelter. Cannot be null or empty.</param>
    /// <param name="contactEmail">The contact email address for the shelter. Cannot be null or empty.</param>
    /// <param name="description">An optional description of the shelter. Can be null.</param>
    /// <param name="capacity">The maximum number of occupants the shelter can accommodate. Must be non-negative.</param>
    /// <param name="currentOccupancy">The current number of occupants in the shelter. Must be non-negative and not exceed capacity.</param>
    /// <param name="photos">An optional list of URLs to photos representing the shelter. Can be null.</param>
    /// <param name="virtualTourUrl">An optional URL to a virtual tour of the shelter. Can be null.</param>
    /// <param name="workingHours">An optional string specifying the shelter's working hours. Can be null.</param>
    /// <param name="socialMedia">An optional dictionary containing social media platform names and their corresponding URLs for the shelter. Can be null.</param>
    /// <param name="managerId">An optional unique identifier of the user to be assigned as the shelter manager. If specified, the user must exist.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created shelter with all details populated.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="managerId"/> is specified but no user with the given identifier is found.</exception>
    Task<Shelter> CreateAsync(
         string name,
         string address,
         double latitude,
         double longitude,
         string contactPhone,
         string contactEmail,
         string? description,
         int capacity,
         int currentOccupancy,
         List<string>? photos,
         string? virtualTourUrl,
         string? workingHours,
         Dictionary<string, string>? socialMedia,
         Guid? managerId,
         CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates the specified shelter in the data store.
    /// </summary>
    /// <param name="shelter">The shelter entity to update. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateAsync(Shelter shelter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes the entity identified by the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

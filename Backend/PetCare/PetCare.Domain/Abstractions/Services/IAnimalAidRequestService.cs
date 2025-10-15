namespace PetCare.Domain.Abstractions.Services;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Provides operations for managing animal aid requests.
/// </summary>
public interface IAnimalAidRequestService
{
    /// <summary>
    /// Adds a new animal aid request created by the user.
    /// </summary>
    /// <param name="user">The user creating the request.</param>
    /// <param name="request">The animal aid request to add.</param>
    /// <param name="requestingUserId">The ID of the user requesting the update. Must match the current user's ID.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAnimalAidRequestAsync(User user, AnimalAidRequest request, Guid requestingUserId);

    /// <summary>
    /// Removes an animal aid request as admin or moderator.
    /// </summary>
    /// <param name="user">The user owning the request.</param>
    /// <param name="requestId">The unique identifier of the animal aid request to remove.</param>
    /// <param name="requestingUserId">The ID of the user requesting the removal.</param>
    /// <exception cref="InvalidOperationException">Thrown when the request is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the requesting user does not have admin or moderator rights.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RemoveAnimalAidRequestAsAdminAsync(User user, Guid requestId, Guid requestingUserId);
}

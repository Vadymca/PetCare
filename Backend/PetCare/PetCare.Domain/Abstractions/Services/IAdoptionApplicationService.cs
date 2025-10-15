namespace PetCare.Domain.Abstractions.Services;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;

/// <summary>
/// Defines operations for managing adoption applications within the system.
/// </summary>
public interface IAdoptionApplicationService
{
    /// <summary>
    /// Adds a new adoption application for the specified user.
    /// </summary>
    /// <param name="user">The user who creates the adoption application.</param>
    /// <param name="application">The adoption application to add.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddAdoptionApplicationAsync(User user, AdoptionApplication application, Guid requestingUserId);

    /// <summary>
    /// Removes an adoption application from the user.
    /// </summary>
    /// <param name="user">The user whose adoption application will be removed.</param>
    /// <param name="applicationId">The ID of the application to remove.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation.</param>
    /// <returns><c>true</c> if the application was removed; otherwise <c>false</c>.</returns>
    Task<bool> RemoveAdoptionApplicationAsync(User user, Guid applicationId, Guid requestingUserId);
}

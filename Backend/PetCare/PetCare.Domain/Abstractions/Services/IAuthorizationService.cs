namespace PetCare.Domain.Abstractions.Services;

using PetCare.Domain.ValueObjects;

/// <summary>
/// Provides authorization checks based on user roles and policies.
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Checks if the user has the required role.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="role">The role to check against the user's roles.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task{Boolean}"/> representing the asynchronous operation,
    /// containing <c>true</c> if the user has the required role; otherwise, <c>false</c>.</returns>
    Task<bool> HasRoleAsync(Guid userId, Role role, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if the user can access a specific shelter.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="shelterId">The unique identifier of the shelter.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task{Boolean}"/> representing the asynchronous operation,
    /// containing <c>true</c> if the user can access the shelter; otherwise, <c>false</c>.</returns>
    Task<bool> CanAccessShelterAsync(Guid userId, Guid shelterId, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if the user can manage a specific animal.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task{Boolean}"/> representing the asynchronous operation,
    /// containing <c>true</c> if the user can manage the animal; otherwise, <c>false</c>.</returns>
    Task<bool> CanManageAnimalAsync(Guid userId, Guid animalId, CancellationToken cancellationToken);
}

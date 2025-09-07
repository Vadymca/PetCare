namespace PetCare.Domain.Abstractions.Repositories;

using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;

/// <summary>
/// Repository interface for accessing user entities.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Retrieves all users with a specific role.
    /// </summary>
    /// <param name="role">The role to filter by.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of users.</returns>
    Task<IReadOnlyList<User>> GetByRoleAsync(UserRole role, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all users who have a subscription to a specific shelter.
    /// </summary>
    /// <param name="shelterId">The shelter's ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of users.</returns>
    Task<IReadOnlyList<User>> GetUsersByShelterSubscriptionAsync(Guid shelterId, CancellationToken cancellationToken = default);
}

namespace PetCare.Domain.Abstractions.Repositories;

using PetCare.Domain.Aggregates;
<<<<<<< HEAD
=======
using PetCare.Domain.Enums;
>>>>>>> 5e60776 (Implementation of repositories for aggregates)

/// <summary>
/// Repository interface for accessing user entities.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the user if found; otherwise, <c>null</c>.
    /// </returns>
<<<<<<< HEAD
    Task<User?> GetByEmailAsync(
        string email, CancellationToken cancellationToken = default);
=======
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

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
>>>>>>> 5e60776 (Implementation of repositories for aggregates)
}

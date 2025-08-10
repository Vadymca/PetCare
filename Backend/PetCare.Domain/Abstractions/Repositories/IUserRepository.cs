namespace PetCare.Domain.Abstractions.Repositories;

using PetCare.Domain.Aggregates;

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
    Task<User?> GetByEmailAsync(
        string email, CancellationToken cancellationToken = default);
}

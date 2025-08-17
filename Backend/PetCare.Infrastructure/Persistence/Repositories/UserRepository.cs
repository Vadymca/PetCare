namespace PetCare.Infrastructure.Persistence.Repositories;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
=======
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.Specifications.User;
>>>>>>> 5e60776 (Implementation of repositories for aggregates)
using PetCare.Infrastructure.Persistence;

/// <summary>
/// Repository implementation for managing <see cref="User"/> entities.
/// </summary>
public class UserRepository : GenericRepository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc/>
<<<<<<< HEAD
    public async Task<User?> GetByEmailAsync(
        string email, CancellationToken cancellationToken = default)
    {
        return await this.Context.Users
            .FirstOrDefaultAsync(
                u =>
            u.Email.Value == email, cancellationToken);
    }
=======
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => this.FindAsync(new UserByEmailSpecification(email), cancellationToken)
               .ContinueWith(t => t.Result.FirstOrDefault(), cancellationToken);

    /// <inheritdoc/>
    public Task<IReadOnlyList<User>> GetByRoleAsync(UserRole role, CancellationToken cancellationToken = default)
        => this.FindAsync(new UsersByRoleSpecification(role), cancellationToken);

    /// <inheritdoc/>
    public Task<IReadOnlyList<User>> GetUsersByShelterSubscriptionAsync(Guid shelterId, CancellationToken cancellationToken = default)
        => this.FindAsync(new UsersByShelterSubscriptionSpecification(shelterId), cancellationToken);
>>>>>>> 5e60776 (Implementation of repositories for aggregates)
}

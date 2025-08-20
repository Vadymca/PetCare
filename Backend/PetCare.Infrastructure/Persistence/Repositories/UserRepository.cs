namespace PetCare.Infrastructure.Persistence.Repositories;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.Specifications.User;
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
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await this.FindAsync(new UserByEmailSpecification(email), cancellationToken)
               .ContinueWith(t => t.Result.FirstOrDefault(), cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<User>> GetByRoleAsync(UserRole role, CancellationToken cancellationToken = default)
        => await this.FindAsync(new UsersByRoleSpecification(role), cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<User>> GetUsersByShelterSubscriptionAsync(Guid shelterId, CancellationToken cancellationToken = default)
        => await this.FindAsync(new UsersByShelterSubscriptionSpecification(shelterId), cancellationToken);
}

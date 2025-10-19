namespace PetCare.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
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
    public async Task<IReadOnlyList<User>> GetByRoleAsync(UserRole role, CancellationToken cancellationToken = default)
        => await this.FindAsync(new UsersByRoleSpecification(role), cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<User>> GetUsersByShelterSubscriptionAsync(Guid shelterId, CancellationToken cancellationToken = default)
        => await this.FindAsync(new UsersByShelterSubscriptionSpecification(shelterId), cancellationToken);

    /// <summary>
    /// Retrieves a paginated list of users with optional search and role filtering.
    /// </summary>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of users per page.</param>
    /// <param name="search">Optional search string to match against Email, FirstName, or LastName.</param>
    /// <param name="role">Optional role name to filter users by <see cref="UserRole"/>.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A tuple containing the list of users and the total count of matching users.</returns>
    public async Task<(IReadOnlyList<User> Users, int TotalCount)> GetUsersAsync(
        int page,
        int pageSize,
        string? search,
        string? role,
        CancellationToken cancellationToken = default)
    {
        var query = this.Context.Set<User>().AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u =>
                EF.Functions.ILike(u.Email!, $"%{search}%") ||
                EF.Functions.ILike(u.FirstName, $"%{search}%") ||
                EF.Functions.ILike(u.LastName, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(role) && Enum.TryParse<UserRole>(role, true, out var roleEnum))
        {
            query = query.Where(u => u.Role == roleEnum);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (users, totalCount);
    }

    /// <summary>
    /// Updates the <see cref="User.Role"/> field directly in the database without tracking conflicts.
    /// </summary>
    /// <param name="userId">The ID of the user whose role is being updated.</param>
    /// <param name="newRole">The new role to assign.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="newRole"/> is invalid.</exception>
    public async Task SetUserRoleAsync(Guid userId, UserRole newRole, CancellationToken cancellationToken = default)
    {
        var user = await this.Context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new KeyNotFoundException($"User with ID '{userId}' not found.");

        user.SetRole(newRole);

        this.Context.Attach(user);
        this.Context.Entry(user).Property(u => u.Role).IsModified = true;
        this.Context.Entry(user).Property(u => u.UpdatedAt).IsModified = true;

        await this.Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves all shelter subscriptions for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose shelter subscriptions are being fetched.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of <see cref="ShelterSubscription"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the user with the given ID does not exist.</exception>
    public async Task<IReadOnlyList<ShelterSubscription>> GetUserShelterSubscriptionsAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        await this.EnsureUserExistsAsync(userId, cancellationToken);

        var user = await this.Context.Users
            .AsNoTracking()
            .Include(u => u.ShelterSubscriptions)
                .ThenInclude(s => s.Shelter) // <- обов'язково Include
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new KeyNotFoundException($"Користувача з Id '{userId}' не знайдено.");

        var sortedSubscriptions = user.ShelterSubscriptions
            .OrderByDescending(s => s.Shelter!.CurrentOccupancy < s.Shelter.Capacity) // приклад сортування по вільних місцях
            .ThenByDescending(s => s.Shelter!.CreatedAt)
            .ToList()
            .AsReadOnly();

        return sortedSubscriptions;
    }

    /// <summary>
    /// Retrieves all animal subscriptions for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose animal subscriptions are being fetched.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of <see cref="AnimalSubscription"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the user with the given ID does not exist.</exception>
    public async Task<IReadOnlyList<AnimalSubscription>> GetUserAnimalSubscriptionsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        await this.EnsureUserExistsAsync(userId, cancellationToken);

        var user = await this.Context.Users
            .AsNoTracking()
            .Include(u => u.AnimalSubscriptions)
                .ThenInclude(s => s.Animal)
                    .ThenInclude(a => a!.Breed)
                        .ThenInclude(b => b!.Specie)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new KeyNotFoundException($"Користувача з Id '{userId}' не знайдено.");

        var sortedSubscriptions = user.AnimalSubscriptions
           .OrderByDescending(s => s.Animal!.Status != AnimalStatus.Dead)
           .ThenByDescending(s => s.Animal!.CreatedAt)
           .ToList()
           .AsReadOnly();

        return sortedSubscriptions;
    }

    /// <summary>
    /// Retrieves all adoption applications submitted by a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose adoption applications are being fetched.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<IReadOnlyList<AdoptionApplication>> GetUserAdoptionApplicationsAsync(
       Guid userId,
       CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<AdoptionApplication>()
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves all events that a specific user is participating in.
    /// </summary>
    /// <param name="userId">The ID of the user whose events are being fetched.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<IReadOnlyList<Event>> GetUserEventsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<Event>()
            .AsNoTracking()
            .Where(e => e.ShelterId != null && e.Participants.Any(p => p.UserId == userId))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Ensures that a user with the given ID exists in the database.
    /// </summary>
    /// <param name="userId">The ID of the user to check.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <exception cref="KeyNotFoundException">Thrown if the user with the given ID does not exist.</exception>
    private async Task EnsureUserExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var exists = await this.Context.Set<User>()
            .AsNoTracking()
            .AnyAsync(u => u.Id == userId, cancellationToken);

        if (!exists)
        {
            throw new KeyNotFoundException($"Користувача з ідентифікатором '{userId}' не знайдено.");
        }
    }
}

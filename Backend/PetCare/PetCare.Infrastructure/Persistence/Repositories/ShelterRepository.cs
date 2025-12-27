namespace PetCare.Infrastructure.Persistence.Repositories;

using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.Search;
using PetCare.Domain.Specifications.Shelter;
using PetCare.Domain.ValueObjects;
using PetCare.Infrastructure.Persistence;

/// <summary>
/// Repository implementation for managing <see cref="Shelter"/> entities.
/// </summary>
public class ShelterRepository : GenericRepository<Shelter>, IShelterRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShelterRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ShelterRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<Shelter?> GetShelterByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken = default)
        => await this.Context.Set<Shelter>()
            .Include(s => s.IoTDevices)
            .Where(new ShelterByDeviceSpecification(deviceId).ToExpression())
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Shelter>> GetByManagerIdAsync(Guid managerId, CancellationToken cancellationToken = default)
        => await this.FindAsync(new SheltersByManagerSpecification(managerId), cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Shelter>> GetWithFreeCapacityAsync(CancellationToken cancellationToken = default)
        => await this.FindAsync(new SheltersWithFreeCapacitySpecification(), cancellationToken);

    /// <inheritdoc />
    public async Task<Shelter?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Slug не може бути порожнім.", nameof(slug));
        }

        var shelter = await this.Context.Set<Shelter>()
            .AsNoTracking()
            .Include(s => s.Animals)
            .Include(s => s.Donations)
            .Include(s => s.VolunteerTasks)
            .Include(s => s.AnimalAidRequests)
            .Include(s => s.IoTDevices)
            .Include(s => s.Events)
            .Include(s => s.Subscribers)
            .FirstOrDefaultAsync(s => s.Slug == Slug.FromExisting(slug), cancellationToken);

        return shelter ?? throw new InvalidOperationException($"Притулок зі slug '{slug}' не знайдено.");
    }

    /// <summary>
    /// Gets a paginated list of shelters ordered by creation date (newest first).
    /// </summary>
    /// <param name="page">The current page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A tuple containing the shelters and total count.</returns>
    public async Task<(IReadOnlyList<Shelter> Shelters, int TotalCount)> GetSheltersAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = this.Context.Set<Shelter>()
            .AsNoTracking()
            .Include(s => s.Manager)
            .Include(s => s.Animals)
            .OrderByDescending(s => s.CreatedAt);

        var total = await query.CountAsync(cancellationToken);

        var shelters = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (shelters, total);
    }

    /// <summary>
    /// Gets a shelter by its unique identifier, including related entities.
    /// </summary>
    /// <param name="id">The unique identifier of the shelter.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The shelter with the specified identifier.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no shelter is found with the specified ID.</exception>
    public new async Task<Shelter?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var shelter = await this.Context.Set<Shelter>()
            .AsNoTracking()
            .Include(s => s.Animals)
            .Include(s => s.VolunteerTasks)
            .Include(s => s.Events)
            .Include(s => s.Donations)
            .Include(s => s.Manager)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return shelter ?? throw new InvalidOperationException($"Притулок з Id '{id}' не знайдено.");
    }

    /// <summary>
    /// Asynchronously adds a new shelter to the data store and retrieves the fully populated shelter entity, including related entities.
    /// </summary>
    /// <param name="shelter">The shelter to add. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>The added shelter entity with related data populated.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the shelter parameter is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the shelter cannot be found in the data store after being added.</exception>
    public async Task<Shelter> AddShelterAsync(Shelter shelter, CancellationToken cancellationToken = default)
    {
        if (shelter == null)
        {
            throw new ArgumentNullException(nameof(shelter), "Притулок не може бути null.");
        }

        await this.AddAsync(shelter, cancellationToken);

        var fullShelter = await this.Context.Shelters
            .Include(s => s.Manager)
            .FirstOrDefaultAsync(s => s.Id == shelter.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Притулок з Id '{shelter.Id}' не знайдено після додавання.");

        return fullShelter;
    }

   /// <summary>
   /// Subscribes a user to the specified shelter asynchronously.
   /// </summary>
   /// <param name="shelterId">The unique identifier of the shelter to which the user will be subscribed.</param>
   /// <param name="userId">The unique identifier of the user to subscribe to the shelter.</param>
   /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
   /// <returns>A task that represents the asynchronous operation. The task result contains the created shelter subscription.</returns>
   /// <exception cref="InvalidOperationException">Thrown if a shelter with the specified shelterId does not exist.</exception>
    public async Task<ShelterSubscription> SubscribeUserAsync(Guid shelterId, Guid userId, CancellationToken cancellationToken = default)
    {
        var shelter = await this.Context.Shelters
            .Include(s => s.Subscribers)
            .FirstOrDefaultAsync(s => s.Id == shelterId, cancellationToken)
            ?? throw new InvalidOperationException($"Притулок з Id '{shelterId}' не знайдено.");

        var subscription = shelter.SubscribeUser(userId);

        this.Context.Set<ShelterSubscription>().Add(subscription);
        await this.Context.SaveChangesAsync(cancellationToken);

        return subscription;
    }

    /// <summary>
    /// Asynchronously removes a user's subscription from the specified shelter.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter from which the user will be unsubscribed.</param>
    /// <param name="userId">The unique identifier of the user to unsubscribe from the shelter.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the unsubscribe operation.</param>
    /// <returns>A task that represents the asynchronous unsubscribe operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a shelter with the specified <paramref name="shelterId"/> does not exist.</exception>
    public async Task UnsubscribeUserAsync(Guid shelterId, Guid userId, CancellationToken cancellationToken = default)
    {
        var shelter = await this.Context.Shelters
            .Include(s => s.Subscribers)
            .FirstOrDefaultAsync(s => s.Id == shelterId, cancellationToken)
            ?? throw new InvalidOperationException($"Притулок з Id '{shelterId}' не знайдено.");

        var subscription = shelter.UnsubscribeUser(userId);

        if (subscription != null)
        {
            this.Context.Set<ShelterSubscription>().Remove(subscription);
        }

        await this.Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Attempts to increment the current occupancy count for the specified shelter if capacity allows.
    /// </summary>
    /// <remarks>If the shelter's current occupancy is equal to or greater than its capacity, the operation
    /// will not succeed and an exception will be thrown. The shelter's last updated timestamp is also set to the
    /// current UTC time upon a successful increment.</remarks>
    /// <param name="shelterId">The unique identifier of the shelter whose occupancy is to be incremented.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the shelter is full or does not exist.</exception>
    public async Task IncrementOccupancyAsync(Guid shelterId, CancellationToken cancellationToken = default)
    {
        var updated = await this.Context.Shelters
            .Where(s => s.Id == shelterId && s.CurrentOccupancy < s.Capacity)
            .ExecuteUpdateAsync(
            s => s
                .SetProperty(x => x.CurrentOccupancy, x => x.CurrentOccupancy + 1)
                .SetProperty(x => x.UpdatedAt, x => DateTime.UtcNow),
            cancellationToken);

        if (updated == 0)
        {
            throw new InvalidOperationException("Притулок заповнений або не знайдено.");
        }
    }

    /// <summary>
    /// Decrements the current occupancy count for the specified shelter if it is greater than zero.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter whose occupancy is to be decremented.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the specified shelter does not exist or its current occupancy is already zero.</exception>
    public async Task DecrementOccupancyAsync(Guid shelterId, CancellationToken cancellationToken = default)
    {
        var updated = await this.Context.Shelters
            .Where(s => s.Id == shelterId && s.CurrentOccupancy > 0)
            .ExecuteUpdateAsync(
            s => s
                .SetProperty(x => x.CurrentOccupancy, x => x.CurrentOccupancy - 1)
                .SetProperty(x => x.UpdatedAt, x => DateTime.UtcNow),
            cancellationToken);

        if (updated == 0)
        {
            throw new InvalidOperationException("Притулок не знайдено або зайнятість вже дорівнює нулю.");
        }
    }

    // ________________________________________AnimalAidRequest________________________________________________

    /// <summary>
    /// Asynchronously retrieves all animal aid requests from the database, including related donations, user, and
    /// shelter information.
    /// </summary>
    /// <remarks>The returned list includes all animal aid requests currently stored in the database. Related
    /// entities are loaded eagerly to provide complete information for each request.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all animal aid requests
    /// with their associated donations, user, and shelter data.</returns>
    public async Task<List<AnimalAidRequest>> GetAllAnimalAidRequestsAsync(CancellationToken cancellationToken = default)
    {
        return await this.Context.AnimalAidRequests
            .Where(a => a.Status != AidStatus.Cancelled)
            .OrderBy(a => a.Status == AidStatus.Open ? 0 : 1)
            .ThenByDescending(a => a.CreatedAt)
            .Include(a => a.Donations)
            .Include(a => a.User)
            .Include(a => a.Shelter)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves an animal aid request by its unique identifier.
    /// </summary>
    /// <remarks>The returned <see cref="AnimalAidRequest"/> includes related donations, user, and shelter
    /// information. If no request with the specified identifier exists, the result is <see langword="null"/>.</remarks>
    /// <param name="id">The unique identifier of the animal aid request to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="AnimalAidRequest"/>
    /// if found; otherwise, <see langword="null"/>.</returns>
    public async Task<AnimalAidRequest> GetAnimalAidRequestByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id не може бути порожнім.", nameof(id));
        }

        var request = await this.Context.AnimalAidRequests
            .Include(a => a.Donations)
            .Include(a => a.User)
            .Include(a => a.Shelter)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        return request ?? throw new InvalidOperationException($"Запит на допомогу з Id '{id}' не знайдено.");
    }

    /// <summary>
    /// Asynchronously retrieves an animal aid request by its unique slug identifier.
    /// </summary>
    /// <remarks>The returned request includes related donations, user, and shelter information. The query is
    /// performed without tracking changes to the entities.</remarks>
    /// <param name="slug">The slug that uniquely identifies the animal aid request. Cannot be null, empty, or consist only of whitespace.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>The animal aid request that matches the specified slug.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="slug"/> is null, empty, or consists only of whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown if no animal aid request with the specified slug is found.</exception>
    public async Task<AnimalAidRequest> GetAnimalAidRequestBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Slug не може бути порожнім.", nameof(slug));
        }

        var request = await this.Context.Set<AnimalAidRequest>()
            .AsNoTracking()
            .Include(a => a.Donations)
            .Include(a => a.User)
            .Include(a => a.Shelter)
            .FirstOrDefaultAsync(a => a.Slug == Slug.FromExisting(slug), cancellationToken);

        return request ?? throw new InvalidOperationException($"Запит на допомогу зі slug '{slug}' не знайдено.");
    }

    /// <summary>
    /// Creates a new animal aid request and saves it to the data store asynchronously.
    /// </summary>
    /// <param name="request">The animal aid request to be created. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>The created <see cref="AnimalAidRequest"/> instance after it has been saved to the data store.</returns>
    public async Task<AnimalAidRequest> CreateAnimalAidRequestAsync(AnimalAidRequest request, CancellationToken cancellationToken = default)
    {
        await this.Context.AnimalAidRequests.AddAsync(request, cancellationToken);
        await this.Context.SaveChangesAsync(cancellationToken);
        return request;
    }

    /// <summary>
    /// Asynchronously updates an existing animal aid request in the data store.
    /// </summary>
    /// <remarks>The update is persisted to the underlying data store when the operation completes. If the
    /// specified request does not exist, no changes are made.</remarks>
    /// <param name="request">The animal aid request entity to update. Must not be null and should represent an existing request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    public async Task UpdateAnimalAidRequestAsync(AnimalAidRequest request, CancellationToken cancellationToken = default)
    {
        this.Context.AnimalAidRequests.Update(request);
        await this.Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Updates the status of an existing animal aid request identified by the specified ID.
    /// </summary>
    /// <remarks>If no animal aid request with the specified ID exists, the method completes without making
    /// any changes.</remarks>
    /// <param name="id">The unique identifier of the animal aid request to update.</param>
    /// <param name="status">The new status to assign to the animal aid request.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous update operation. The task completes when the status has been updated or
    /// if the request does not exist.</returns>
    public async Task UpdateAnimalAidRequestStatusAsync(Guid id, AidStatus status, CancellationToken cancellationToken = default)
    {
        var request = await this.GetAnimalAidRequestByIdAsync(id, cancellationToken);
        if (request is null)
        {
            return;
        }

        request.UpdateStatus(status);
        this.Context.AnimalAidRequests.Update(request);
        await this.Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously deletes the animal aid request identified by the specified ID, if it exists.
    /// </summary>
    /// <remarks>If no animal aid request with the specified ID exists, the method completes without
    /// performing any action. This method does not throw an exception if the request is not found.</remarks>
    /// <param name="id">The unique identifier of the animal aid request to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    public async Task DeleteAnimalAidRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var request = await this.GetAnimalAidRequestByIdAsync(id, cancellationToken);
        if (request is null)
        {
            return;
        }

        this.Context.AnimalAidRequests.Remove(request);
        await this.Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves a list of urgent animal aid requests, ordered by creation date in descending order.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of urgent animal aid
    /// requests, with the most recently created requests first. If no urgent requests exist, the list will be empty.</returns>
    public async Task<List<AnimalAidRequest>> GetUrgentAnimalAidRequestsAsync(CancellationToken cancellationToken = default)
    {
        return await this.Context.AnimalAidRequests
            .Where(a => a.IsUrgent)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<decimal> SumCompletedByAidRequestIdAsync(
    Guid aidRequestId,
    CancellationToken cancellationToken = default)
    {
        return await this.Context.Donations
            .Where(d =>
                d.TargetEntity == "AidRequest" &&
                d.TargetEntityId == aidRequestId &&
                d.Status == DonationStatus.Completed)
            .SumAsync(d => d.Amount, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<int> GetDonationsCountAsync(Guid aidRequestId, CancellationToken cancellationToken = default)
    {
        return await this.Context.Donations
            .CountAsync(d => d.TargetEntity == "AidRequest" && d.TargetEntityId == aidRequestId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<SearchItem>> SearchShelterAsync(
    string query,
    int limit,
    CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
        {
            return Array.Empty<SearchItem>();
        }

        var tsQuery = query.Trim() + ":*";

        var sql = @$"
            SELECT ""Name"", ""Slug"", ""Description"" AS ""Snippet""
            FROM ""Shelters""
            WHERE ""SearchVector"" @@ to_tsquery('simple', {{0}})
               OR ""SearchVector"" @@ to_tsquery('english', {{0}})
            ORDER BY
                ts_rank(""SearchVector"", to_tsquery('simple', {{0}})) DESC,
                ts_rank(""SearchVector"", to_tsquery('english', {{0}})) DESC,
                ""UpdatedAt"" DESC,
                ""CreatedAt"" DESC
            LIMIT {{1}};
        ";

        return await this.Context.SearchItems
            .FromSqlInterpolated(FormattableStringFactory.Create(sql, tsQuery, limit))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<SearchItem>> SearchProjectAsync(
    string query,
    int limit,
    CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
        {
            return Array.Empty<SearchItem>();
        }

        var tsQuery = query.Trim() + ":*";

        var sql = @$"
            SELECT ""Title"" AS ""Name"", ""Slug"", ""Description"" AS ""Snippet""
            FROM ""AnimalAidRequests""
            WHERE ""SearchVector"" @@ to_tsquery('simple', {{0}})
               OR ""SearchVector"" @@ to_tsquery('english', {{0}})
            ORDER BY
                ts_rank(""SearchVector"", to_tsquery('simple', {{0}})) DESC,
                ts_rank(""SearchVector"", to_tsquery('english', {{0}})) DESC,
                ""UpdatedAt"" DESC,
                ""CreatedAt"" DESC
            LIMIT {{1}};
        ";

        return await this.Context.SearchItems
            .FromSqlInterpolated(FormattableStringFactory.Create(sql, tsQuery, limit))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

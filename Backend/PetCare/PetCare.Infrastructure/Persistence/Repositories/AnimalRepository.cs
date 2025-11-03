namespace PetCare.Infrastructure.Persistence.Repositories;

using System.Threading;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.Specifications.Animal;
using PetCare.Domain.ValueObjects;
using PetCare.Infrastructure.Persistence;

/// <summary>
/// Repository for managing <see cref="Animal"/> aggregate.
/// </summary>
public class AnimalRepository : GenericRepository<Animal>, IAnimalRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public AnimalRepository(
        AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Animal>> GetByShelterIdAsync(Guid shelterId, CancellationToken cancellationToken = default)
        => await this.FindAsync(new AnimalsByShelterSpecification(shelterId), cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Animal>> GetByBreedIdAsync(Guid breedId, CancellationToken cancellationToken = default)
        => await this.FindAsync(new AnimalsByBreedSpecification(breedId), cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<Animal>> GetAvailableForAdoptionAsync(CancellationToken cancellationToken = default)
        => await this.FindAsync(new AvailableAnimalsSpecification(), cancellationToken);

    /// <summary>
    /// Gets an animal by its unique slug, including related entities.
    /// </summary>
    /// <param name="slug">The slug of the animal.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<Animal?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Slug не може бути порожнім.", nameof(slug));
        }

        var animal = await this.Context.Set<Animal>()
            .AsNoTracking()
            .Include(a => a.Breed)
                .ThenInclude(b => b!.Specie)
            .Include(a => a.Shelter)
            .FirstOrDefaultAsync(a => a.Slug == Slug.FromExisting(slug), cancellationToken);

        return animal ?? throw new InvalidOperationException($"Тварину зі slug '{slug}' не знайдено.");
    }

    /// <summary>
    /// Gets a paginated list of animals with optional filtering by shelter, breed, specie, and search term.
    /// </summary>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sizes">The sizes of the animal to filter by (optional).</param>
    /// <param name="genders">The genders of the animal to filter by (optional).</param>
    /// <param name="minAge">The minimum age of the animal to filter by (optional).</param>
    /// <param name="maxAge">The maximum age of the animal to filter by (optional).</param>
    /// <param name="careCosts">The care costs of the animal to filter by (optional).</param>
    /// <param name="isSterilized">Whether the animal is sterilized to filter by (optional).</param>
    /// <param name="isUnderCare">Whether the animal is under care to filter by (optional).</param>
    /// <param name="shelterId">The unique identifier of the shelter to filter by (optional).</param>
    /// <param name="statuses">The statuses of the animal to filter by (optional).</param>
    /// <param name="specieId">The unique identifier of the specie to filter by (optional).</param>
    /// <param name="breedId">The unique identifier of the breed to filter by (optional).</param>
    /// <param name="search">The search term to filter by name or description (optional).</param>
    /// <param name="animalTypeFilter">The type of animal to filter by (optional).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<(IReadOnlyList<Animal> Animals, int TotalCount)> GetAnimalsAsync(
    int page,
    int pageSize,
    IEnumerable<AnimalSize>? sizes = null,
    IEnumerable<AnimalGender>? genders = null,
    int? minAge = null,
    int? maxAge = null,
    IEnumerable<AnimalCareCost>? careCosts = null,
    bool? isSterilized = null,
    bool? isUnderCare = null,
    Guid? shelterId = null,
    IEnumerable<AnimalStatus>? statuses = null,
    Guid? specieId = null,
    Guid? breedId = null,
    string? search = null,
    string? animalTypeFilter = null,
    CancellationToken cancellationToken = default)
    {
        var query = this.Context.Set<Animal>()
    .AsNoTracking()
    .Include(a => a.Breed)
        .ThenInclude(b => b!.Specie)
    .Include(a => a.Shelter)
    .AsQueryable();

        // Фільтри по енумам та nullable полях, які EF Core може перекласти
        if (sizes is { } sizeList && sizeList.Any())
        {
            query = query.Where(a => sizeList.Contains(a.Size));
        }

        if (genders is { } genderList && genderList.Any())
        {
            query = query.Where(a => genderList.Contains(a.Gender));
        }

        if (careCosts is { } careCostList && careCostList.Any())
        {
            query = query.Where(a => careCostList.Contains(a.CareCost));
        }

        if (isSterilized.HasValue)
        {
            query = query.Where(a => a.IsSterilized == isSterilized.Value);
        }

        if (isUnderCare.HasValue)
        {
            query = query.Where(a => a.IsUnderCare == isUnderCare.Value);
        }

        if (shelterId.HasValue)
        {
            query = query.Where(a => a.ShelterId == shelterId.Value);
        }

        if (statuses is { } statusList && statusList.Any())
        {
            query = query.Where(a => statusList.Contains(a.Status));
        }

        if (specieId.HasValue)
        {
            query = query.Where(a => a.Breed!.SpeciesId == specieId.Value);
        }

        if (breedId.HasValue)
        {
            query = query.Where(a => a.BreedId == breedId.Value);
        }

        // Завантажуємо із бази
        var animalsList = await query.ToListAsync(cancellationToken);

        // Client-side фільтрація по типу виду
        if (!string.IsNullOrWhiteSpace(animalTypeFilter))
        {
            var normalized = animalTypeFilter.Trim().ToLower();

            animalsList = normalized switch
            {
                "cats" => animalsList.Where(a =>
                    a.Breed?.Specie?.Name.Value.Equals("Кішка", StringComparison.OrdinalIgnoreCase) ?? false).ToList(),

                "dogs" => animalsList.Where(a =>
                    a.Breed?.Specie?.Name.Value.Equals("Собака", StringComparison.OrdinalIgnoreCase) ?? false).ToList(),

                "others" => animalsList.Where(a =>
                    !(a.Breed?.Specie?.Name.Value.Equals("Кішка", StringComparison.OrdinalIgnoreCase) ?? false) &&
                    !(a.Breed?.Specie?.Name.Value.Equals("Собака", StringComparison.OrdinalIgnoreCase) ?? false)).ToList(),

                _ => animalsList,
            };
        }

        // Client-side фільтрація Birthday (Value Object)
        if (minAge.HasValue)
        {
            var minBirthday = DateTime.UtcNow.AddYears(-minAge.Value);
            animalsList = animalsList.Where(a => a.Birthday != null && a.Birthday.Value <= minBirthday).ToList();
        }

        if (maxAge.HasValue)
        {
            var maxBirthday = DateTime.UtcNow.AddYears(-maxAge.Value);
            animalsList = animalsList.Where(a => a.Birthday != null && a.Birthday.Value >= maxBirthday).ToList();
        }

        // Client-side пошук по VO Name.Value та Description
        if (!string.IsNullOrWhiteSpace(search))
        {
            var tsQuery = search.Trim().ToLower();
            animalsList = animalsList
                .Where(a =>
                    (a.Name?.Value?.ToLower().Contains(tsQuery) ?? false) ||
                    (a.Description?.ToLower().Contains(tsQuery) ?? false))
                .ToList();
        }

        // Сортування: новіші спочатку, мертві в кінець
        animalsList = animalsList
            .OrderByDescending(a => a.Status != AnimalStatus.Dead)
            .ThenByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var total = animalsList.Count;

        return (animalsList, total);
    }

    /// <summary>
    /// Gets an animal by its unique identifier, including related entities.
    /// </summary>
    /// <param name="id">The unique identifier of the animal.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public new async Task<Animal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var animal = await this.Context.Set<Animal>()
            .AsNoTracking()
            .Include(a => a.Breed)
                .ThenInclude(b => b!.Specie)
            .Include(a => a.Shelter)
            .Include(a => a.AdoptionApplications)
            .Include(a => a.Tags)
            .Include(a => a.SuccessStories)
            .Include(a => a.Subscribers)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        return animal ?? throw new InvalidOperationException($"Тварину з Id '{id}' не знайдено.");
    }

    /// <summary>
    /// Asynchronously adds a new animal to the data store and retrieves the fully populated animal entity, including
    /// related breed, species, and shelter information.
    /// </summary>
    /// <param name="animal">The animal to add. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added animal entity with related
    /// breed, species, and shelter data populated.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the animal parameter is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the animal cannot be found in the data store after being added.</exception>
    public async Task<Animal> AddAnimalAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        if (animal == null)
        {
            throw new ArgumentNullException(nameof(animal), "Тварина не може бути null.");
        }

        await this.AddAsync(animal, cancellationToken);

        var fullAnimal = await this.Context.Animals
            .Include(a => a.Breed)
                .ThenInclude(b => b!.Specie)
            .Include(a => a.Shelter)
            .FirstOrDefaultAsync(a => a.Id == animal.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Тварину з Id '{animal.Id}' не знайдено після додавання.");

        return fullAnimal;
    }

    /// <summary>
    /// Subscribes the specified user to the animal with the given identifier asynchronously.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal to which the user will be subscribed.</param>
    /// <param name="userId">The unique identifier of the user to subscribe to the animal.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created AnimalSubscription
    /// representing the user's subscription to the animal.</returns>
    /// <exception cref="InvalidOperationException">Thrown if an animal with the specified animalId does not exist.</exception>
    public async Task<AnimalSubscription> SubscribeUserAsync(Guid animalId, Guid userId, CancellationToken cancellationToken = default)
    {
        var animal = await this.Context.Animals
            .Include(a => a.Subscribers)
            .FirstOrDefaultAsync(a => a.Id == animalId, cancellationToken)
            ?? throw new InvalidOperationException($"Тварину з Id '{animalId}' не знайдено.");

        var subscription = animal.SubscribeUser(userId);

        this.Context.Set<AnimalSubscription>().Add(subscription);
        await this.Context.SaveChangesAsync(cancellationToken);

        return subscription;
    }

    /// <summary>
    /// Unsubscribes a user from an animal by ID.
    /// </summary>
    /// /// <param name="animalId">The unique identifier of the animal.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task UnsubscribeUserAsync(Guid animalId, Guid userId, CancellationToken cancellationToken = default)
    {
        var animal = await this.Context.Animals
            .Include(a => a.Subscribers)
            .FirstOrDefaultAsync(a => a.Id == animalId, cancellationToken)
            ?? throw new InvalidOperationException($"Тварину з Id '{animalId}' не знайдено.");

        var subscription = animal.UnsubscribeUser(userId);

        if (subscription != null)
        {
            this.Context.Set<AnimalSubscription>().Remove(subscription);
        }

        await this.Context.SaveChangesAsync(cancellationToken);
    }
}

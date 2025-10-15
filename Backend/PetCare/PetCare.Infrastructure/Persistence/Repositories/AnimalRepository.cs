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
    private readonly IFileStorageService fileStorageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="fileStorageService">The file storage service for handling animal photos.</param>
    public AnimalRepository(
        AppDbContext context,
        IFileStorageService fileStorageService)
        : base(context)
    {
        this.fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
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
    /// Creates a new animal with the specified parameters.
    /// </summary>
    /// <param name="userId">The unique identifier of the user creating the animal.</
    /// <param name="name">The name of the animal.</param>
    /// <param name="breedId">The unique identifier of the breed of the animal.</param>
    /// <param name="birthday">The birthday of the animal (optional).</param>
    /// <param name="gender">The gender of the animal.</param>
    /// <param name="description">The description of the animal (optional).</param>
    /// <param name="healthConditions">A list of health conditions of the animal (optional).</param>
    /// <param name="specialNeeds">A list of special needs of the animal (optional).</param>
    /// <param name="temperaments">A list of temperaments of the animal (optional).</param>
    /// <param name="size">The size of the animal.</param>
    /// <param name="photos">A list of photo URLs of the animal (optional).</param>
    /// <param name="videos">A list of video URLs of the animal (optional).</param>
    /// <param name="shelterId">The unique identifier of the shelter where the animal is located.</param>
    /// <param name="status">The adoption status of the animal.</param>
    /// <param name="careCost">The expected care cost of the animal.</param>
    /// <param name="adoptionRequirements">The adoption requirements for the animal (optional).</param>
    /// <param name="microchipId">The microchip ID of the animal (optional).</param>
    /// <param name="weight">The weight of the animal in kilograms (optional).</param>
    /// <param name="height">The height of the animal in centimeters (optional).</param>
    /// <param name="color">The color of the animal, if any. Can be null.</param>
    /// <param name="isSterilized">Indicates whether the animal is sterilized.</param>
    /// <param name="isUnderCare">Indicates whether the animal is under care.</param>
    /// <param name="haveDocuments">Indicates whether the animal has documents.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The created animal.</returns>
    public async Task<Animal> CreateAsync(
    Guid userId,
    string name,
    Guid breedId,
    Birthday? birthday,
    AnimalGender gender,
    string? description,
    List<string>? healthConditions,
    List<string>? specialNeeds,
    List<AnimalTemperament>? temperaments,
    AnimalSize size,
    List<string>? photos,
    List<string>? videos,
    Guid shelterId,
    AnimalStatus status,
    AnimalCareCost careCost,
    string? adoptionRequirements,
    string? microchipId,
    float? weight,
    float? height,
    string? color,
    bool isSterilized,
    bool isUnderCare,
    bool haveDocuments,
    CancellationToken cancellationToken = default)
    {
        var animal = Animal.Create(
            userId,
            name,
            breedId,
            birthday,
            gender,
            description,
            healthConditions,
            specialNeeds,
            temperaments,
            size,
            photos,
            videos,
            shelterId,
            status,
            careCost,
            adoptionRequirements,
            microchipId,
            weight,
            height,
            color,
            isSterilized,
            isUnderCare,
            haveDocuments);

        await this.AddAsync(animal, cancellationToken);

        var fullAnimal = await this.Context.Animals
        .Include(a => a.Breed)
            .ThenInclude(b => b!.Specie)
        .Include(a => a.Shelter)
        .FirstAsync(a => a.Id == animal.Id, cancellationToken);

        return fullAnimal;
    }

    /// <summary>
    /// Adds a photo URL to the specified animal.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <param name="photoUrl">The URL of the photo to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AddPhotoAsync(Guid animalId, string photoUrl, CancellationToken cancellationToken = default)
    {
        var animal = await this.GetByIdAsync(animalId, cancellationToken)
            ?? throw new InvalidOperationException($"Тварину з Id '{animalId}' не знайдено.");

        animal.AddPhoto(photoUrl);

        await this.UpdateAsync(animal, cancellationToken);
    }

    /// <summary>
    /// Removes a photo URL from the specified animal.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <param name="photoUrl">The URL of the photo to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation,
    /// containing <c>true</c> if the photo was removed; otherwise, <c>false</c>.</returns>
    public async Task<bool> RemovePhotoAsync(Guid animalId, string photoUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
        {
            return false;
        }

        var animal = await this.GetByIdAsync(animalId, cancellationToken)
            ?? throw new InvalidOperationException($"Тварину з Id '{animalId}' не знайдено.");

        var removed = animal.RemovePhoto(photoUrl);

        if (removed)
        {
            await this.UpdateAsync(animal, cancellationToken);
            await this.fileStorageService.DeleteAsync(photoUrl);
        }

        return removed;
    }

    /// <summary>
    /// Subscribes a user to an animal by ID.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SubscribeUserAsync(Guid animalId, Guid userId, CancellationToken cancellationToken = default)
    {
        var animal = await this.Context.Animals
            .Include(a => a.Subscribers)
            .FirstOrDefaultAsync(a => a.Id == animalId, cancellationToken)
            ?? throw new InvalidOperationException($"Тварину з Id '{animalId}' не знайдено.");

        var subscription = animal.SubscribeUser(userId);

        this.Context.Set<AnimalSubscription>().Add(subscription);

        await this.Context.SaveChangesAsync(cancellationToken);
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

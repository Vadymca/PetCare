namespace PetCare.Infrastructure.Services;

using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Provides operations for managing and retrieving animal-related data.
/// </summary>
public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository animalRepository;
    private readonly IShelterRepository shelterRepository;
    private readonly IStorageService storageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalService"/> class with the specified animal repository and file storage.
    /// service.
    /// </summary>
    /// <param name="animalRepository">The repository used to manage animal data. Cannot be null.</param>
    /// <param name="shelterRepository">The repository used to manage shelter data. Cannot be null.</param>
    /// <param name="storageService">The service used for file storage operations related to animals. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if animalRepository or fileStorageService is null.</exception>
    public AnimalService(
        IAnimalRepository animalRepository,
        IShelterRepository shelterRepository,
        IStorageService storageService)
    {
        this.animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
        this.shelterRepository = shelterRepository ?? throw new ArgumentNullException(nameof(shelterRepository));
        this.storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
    }

        /// <summary>
        /// Asynchronously retrieves an animal by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the animal to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the animal with the specified
        /// identifier, or <see langword="null"/> if no animal is found.</returns>
    public async Task<Animal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await this.animalRepository.GetByIdAsync(id, cancellationToken);

    /// <summary>
    /// Asynchronously retrieves an animal entity that matches the specified slug.
    /// </summary>
    /// <param name="slug">The unique slug identifier of the animal to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the animal entity if found;
    /// otherwise, <see langword="null"/>.</returns>
    public async Task<Animal?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => await this.animalRepository.GetBySlugAsync(slug, cancellationToken);

    /// <summary>
    /// Asynchronously retrieves a paged list of animals that match the specified filter criteria.
    /// </summary>
    /// <remarks>The returned list contains only the animals for the specified page and page size, but the
    /// total count reflects all animals matching the filters across all pages. Use the total count to implement paging
    /// in client applications.</remarks>
    /// <param name="page">The zero-based page index of the results to retrieve. Must be greater than or equal to 0.</param>
    /// <param name="pageSize">The maximum number of animals to include in the returned page. Must be greater than 0.</param>
    /// <param name="sizes">An optional collection of animal sizes to filter the results. If null, animals of all sizes are included.</param>
    /// <param name="genders">An optional collection of animal genders to filter the results. If null, animals of all genders are included.</param>
    /// <param name="minAge">The optional minimum age, in years, of animals to include. If null, no minimum age filter is applied.</param>
    /// <param name="maxAge">The optional maximum age, in years, of animals to include. If null, no maximum age filter is applied.</param>
    /// <param name="careCosts">An optional collection of care cost categories to filter the results. If null, animals with any care cost are
    /// included.</param>
    /// <param name="isSterilized">An optional value indicating whether to include only sterilized or only non-sterilized animals. If null, both
    /// are included.</param>
    /// <param name="isUnderCare">An optional value indicating whether to include only animals currently under care. If null, both are included.</param>
    /// <param name="shelterId">The optional unique identifier of a shelter to filter animals by their shelter. If null, animals from all
    /// shelters are included.</param>
    /// <param name="statuses">An optional collection of animal statuses to filter the results. If null, animals of all statuses are included.</param>
    /// <param name="specieId">The optional unique identifier of a species to filter the results. If null, animals of all species are included.</param>
    /// <param name="breedId">The optional unique identifier of a breed to filter the results. If null, animals of all breeds are included.</param>
    /// <param name="search">An optional search string to filter animals by name or other searchable fields. If null or empty, no search
    /// filter is applied.</param>
    /// <param name="animalTypeFilter">An optional string to filter animals by type (e.g., "Dog", "Cat"). If null or empty, no type filter is applied.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with a read-only list of
    /// animals matching the filter criteria and the total count of animals that match the filters.</returns>
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
        return await this.animalRepository.GetAnimalsAsync(
            page,
            pageSize,
            sizes,
            genders,
            minAge,
            maxAge,
            careCosts,
            isSterilized,
            isUnderCare,
            shelterId,
            statuses,
            specieId,
            breedId,
            search,
            animalTypeFilter,
            cancellationToken);
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
        var shelter = await this.shelterRepository.GetByIdAsync(shelterId, cancellationToken)
            ?? throw new KeyNotFoundException($"Притулок з Id '{shelterId}' не знайдено.");

        if (!shelter.HasFreeCapacity())
        {
            throw new InvalidOperationException($"Притулок '{shelter.Name.Value}' переповнений — додавання тварини неможливе.");
        }

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

        var addedAnimal = await this.animalRepository.AddAnimalAsync(animal, cancellationToken);

        // Збільшуємо кількість тварин у притулку без дубль-трекінгу
        await this.shelterRepository.IncrementOccupancyAsync(shelterId, cancellationToken);

        return addedAnimal;
    }

    /// <summary>
    /// Asynchronously updates the specified animal in the data store.
    /// </summary>
    /// <param name="animal">The animal entity to update. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    public async Task UpdateAsync(Animal animal, CancellationToken cancellationToken = default)
        => await this.animalRepository.UpdateAsync(animal, cancellationToken);

    /// <summary>
    /// Asynchronously deletes the animal with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the animal to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no animal with the specified identifier is found.</exception>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var animal = await this.animalRepository.GetByIdAsync(id, cancellationToken);
        if (animal is null)
        {
            throw new InvalidOperationException($"Тварину з Id '{id}' не знайдено.");
        }

        await this.animalRepository.DeleteAsync(animal, cancellationToken);
        await this.shelterRepository.DecrementOccupancyAsync(animal.ShelterId, cancellationToken);
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
        var animal = await this.animalRepository.GetByIdAsync(animalId, cancellationToken)
            ?? throw new InvalidOperationException($"Тварину з Id '{animalId}' не знайдено.");

        animal.AddPhoto(photoUrl);

        await this.animalRepository.UpdateAsync(animal, cancellationToken);
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
        var animal = await this.animalRepository.GetByIdAsync(animalId, cancellationToken)
            ?? throw new InvalidOperationException($"Тварину з Id '{animalId}' не знайдено.");

        var removed = animal.RemovePhoto(photoUrl);

        if (removed)
        {
            await this.animalRepository.UpdateAsync(animal, cancellationToken);
            var objectName = Path.GetFileName(photoUrl);
            await this.storageService.DeleteFileAsync(objectName);
        }

        return removed;
    }

    /// <summary>
    /// Subscribes a user to updates for the specified animal asynchronously.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal to which the user will be subscribed.</param>
    /// <param name="userId">The unique identifier of the user to subscribe.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an AnimalSubscription object
    /// representing the user's subscription to the animal.</returns>
    public async Task<AnimalSubscription> SubscribeUserAsync(Guid animalId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await this.animalRepository.SubscribeUserAsync(animalId, userId, cancellationToken);
    }

    /// <summary>
    /// Asynchronously removes a user's subscription to notifications or updates for the specified animal.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal from which the user will be unsubscribed.</param>
    /// <param name="userId">The unique identifier of the user to unsubscribe.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the unsubscribe operation.</param>
    /// <returns>A task that represents the asynchronous unsubscribe operation.</returns>
    public async Task UnsubscribeUserAsync(Guid animalId, Guid userId, CancellationToken cancellationToken = default)
    {
        await this.animalRepository.UnsubscribeUserAsync(animalId, userId, cancellationToken);
    }
}

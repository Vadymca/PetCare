namespace PetCare.Application.Interfaces;

using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Defines the contract for services that provide operations related to animals.
/// </summary>
public interface IAnimalService
{
    /// <summary>
    /// Asynchronously retrieves an animal by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the animal to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the animal with the specified
    /// identifier, or <see langword="null"/> if no animal is found.</returns>
    Task<Animal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves an animal by its unique slug identifier.
    /// </summary>
    /// <param name="slug">The unique slug that identifies the animal to retrieve. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Animal"/> if found;
    /// otherwise, <see langword="null"/>.</returns>
    Task<Animal?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of animals with optional filtering by size, gender, age, care cost,
    /// sterilization status, shelter, specie, breed, status, and search term.
    /// </summary>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sizes">The sizes of the animal to filter by (optional).</param>
    /// <param name="genders">The genders of the animal to filter by (optional).</param>
    /// <param name="minAge">The minimum age of the animal in years (optional).</param>
    /// <param name="maxAge">The maximum age of the animal in years (optional).</param>
    /// <param name="careCosts">The expected care costs of the animal to filter by (optional).</param>
    /// <param name="isSterilized">Whether the animal is sterilized (optional).</param>
    /// <param name="isUnderCare">Whether the animal is under care to filter by (optional).</param>
    /// <param name="shelterId">The unique identifier of the shelter to filter by (optional).</param>
    /// <param name="statuses">The adoption statuses of the animal to filter by (optional).</param>
    /// <param name="specieId">The unique identifier of the specie to filter by (optional).</param>
    /// <param name="breedId">The unique identifier of the breed to filter by (optional).</param>
    /// <param name="search">The search term to filter by name or description (optional).</param>
    /// <param name="animalTypeFilter">The type of animal to filter by (e.g., "Dog", "Cat") (optional).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A tuple containing a read-only list of animals and the total count of animals matching the criteria.
    /// </returns>
    Task<(IReadOnlyList<Animal> Animals, int TotalCount)> GetAnimalsAsync(
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
        CancellationToken cancellationToken = default);

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
    Task<Animal> CreateAsync(
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
    CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates the specified animal in the data store.
    /// </summary>
    /// <param name="animal">The animal entity to update. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateAsync(Animal animal, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes the entity identified by the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously adds a photo to the specified animal's profile.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal to which the photo will be added.</param>
    /// <param name="photoUrl">The URL of the photo to add. Must be a valid, accessible URL.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    Task AddPhotoAsync(Guid animalId, string photoUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously removes a photo associated with the specified animal.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal whose photo is to be removed.</param>
    /// <param name="photoUrl">The URL of the photo to remove. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the photo was
    /// successfully removed; otherwise, <see langword="false"/>.</returns>
    Task<bool> RemovePhotoAsync(Guid animalId, string photoUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Subscribes the specified user to updates for the given animal asynchronously.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal to which the user will be subscribed.</param>
    /// <param name="userId">The unique identifier of the user who will be subscribed to the animal.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an AnimalSubscription object
    /// representing the user's subscription to the animal.</returns>
    Task<AnimalSubscription> SubscribeUserAsync(Guid animalId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously removes a user's subscription to notifications for the specified animal.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal for which the user will be unsubscribed.</param>
    /// <param name="userId">The unique identifier of the user to unsubscribe from the animal's notifications.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the unsubscribe operation.</param>
    /// <returns>A task that represents the asynchronous unsubscribe operation.</returns>
    Task UnsubscribeUserAsync(Guid animalId, Guid userId, CancellationToken cancellationToken = default);
}

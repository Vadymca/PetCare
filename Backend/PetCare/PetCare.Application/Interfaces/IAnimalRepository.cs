namespace PetCare.Application.Interfaces;

using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a repository interface for accessing animal entities.
/// </summary>
public interface IAnimalRepository : IRepository<Animal>
{
    /// <summary>
    /// Retrieves all animals in a specific shelter.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of animals.</returns>
    Task<IReadOnlyList<Animal>> GetByShelterIdAsync(Guid shelterId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all animals of a specific breed.
    /// </summary>
    /// <param name="breedId">The unique identifier of the breed.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of animals.</returns>
    Task<IReadOnlyList<Animal>> GetByBreedIdAsync(Guid breedId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all available animals for adoption.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of available animals.</returns>
    Task<IReadOnlyList<Animal>> GetAvailableForAdoptionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an animal by its unique slug.
    /// </summary>
    /// <param name="slug">The slug of the animal.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the animal if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Animal?> GetBySlugAsync(
        string slug, CancellationToken cancellationToken = default);

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
    /// Adds a photo URL to the specified animal.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <param name="photoUrl">The URL of the photo to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddPhotoAsync(Guid animalId, string photoUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a photo URL from the specified animal.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <param name="photoUrl">The URL of the photo to remove.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>True if the photo was successfully removed; otherwise, false.</returns>
    Task<bool> RemovePhotoAsync(Guid animalId, string photoUrl, CancellationToken cancellationToken = default);

    /// <summary>
    /// Subscribes a user to an animal by ID.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SubscribeUserAsync(Guid animalId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unsubscribes a user from an animal by ID.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UnsubscribeUserAsync(Guid animalId, Guid userId, CancellationToken cancellationToken = default);
}

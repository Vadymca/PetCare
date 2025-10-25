namespace PetCare.Application.Interfaces;

using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;

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
    /// <param name="animalTypeFilter">The type of animal to filter by (optional).</param>
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
    /// Asynchronously adds a new animal to the data store.
    /// </summary>
    /// <param name="animal">The animal to add. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added animal, including any
    /// updated properties such as its assigned identifier.</returns>
    Task<Animal> AddAnimalAsync(Animal animal, CancellationToken cancellationToken = default);

    /// <summary>
    /// Subscribes the specified user to updates for the given animal asynchronously.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal to which the user will be subscribed.</param>
    /// <param name="userId">The unique identifier of the user to subscribe to the animal updates.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an AnimalSubscription object
    /// representing the user's subscription to the animal.</returns>
    Task<AnimalSubscription> SubscribeUserAsync(Guid animalId, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unsubscribes a user from an animal by ID.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UnsubscribeUserAsync(Guid animalId, Guid userId, CancellationToken cancellationToken = default);
}

namespace PetCare.Domain.Abstractions.Repositories;

using PetCare.Domain.Aggregates;

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
}

namespace PetCare.Domain.Abstractions.Repositories;

using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Repository interface for accessing species entities.
/// </summary>
public interface ISpeciesRepository : IRepository<Specie>
{
    /// <summary>
    /// Retrieves a species entity by its name.
    /// </summary>
    /// <param name="name">The name of the species.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the species if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Specie?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a read-only list of breeds associated with the specified species.
    /// </summary>
    /// <param name="specieId">The unique identifier of the species for which to retrieve breeds.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of breeds for the
    /// specified species. The list will be empty if no breeds are found.</returns>
    Task<IReadOnlyList<Breed>> GetBreedsAsync(Guid specieId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all breeds across all species.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A read-only list of breeds.</returns>
    Task<IReadOnlyList<Breed>> GetAllBreedsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously retrieves the breed with the specified unique identifier.
    /// </summary>
    /// <param name="breedId">The unique identifier of the breed to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the breed with the specified
    /// identifier, or null if no such breed exists.</returns>
    Task<Breed> GetBreedByIdAsync(Guid breedId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the species associated with the specified breed identifier.
    /// </summary>
    /// <param name="breedId">The unique identifier of the breed for which to retrieve the species.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the species associated with the
    /// specified breed, or <c>null</c> if no matching species is found.</returns>
    Task<Specie?> GetSpecieWithBreedAsync(Guid breedId, CancellationToken cancellationToken = default);
}

namespace PetCare.Application.Interfaces;

using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Defines the contract for operations related to species management within the application.
/// </summary>
/// <remarks>Implementations of this interface should provide methods for creating, retrieving, updating, and
/// deleting species data. This interface is intended to be used as an abstraction for dependency injection and testing
/// purposes.</remarks>
public interface ISpecieService
{
    /// <summary>
    /// Asynchronously retrieves a read-only list of all available species.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of species. The
    /// list will be empty if no species are available.</returns>
    Task<IReadOnlyList<Specie>> GetAllSpeciesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a species entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the species to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the species entity matching the
    /// specified identifier, or null if no such species exists.</returns>
    Task<Specie> GetSpeciesByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a read-only list of breeds associated with the specified species.
    /// </summary>
    /// <param name="specieId">The unique identifier of the species for which to retrieve breeds.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of breeds for the
    /// specified species. If no breeds are found, the list will be empty.</returns>
    Task<IReadOnlyList<Breed>> GetBreedsAsync(Guid specieId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new species with the specified name asynchronously.
    /// </summary>
    /// <param name="name">The name of the species to create. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created species.</returns>
    Task<Specie> CreateSpeciesAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates the name of the species identified by the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the species to update.</param>
    /// <param name="newName">The new name to assign to the species. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated species object.</returns>
    Task<Specie> UpdateSpeciesAsync(Guid id, string newName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes the species identified by the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the species to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteSpeciesAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a collection of all available dog breeds.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="GetAllBreedsResponseDto"/> with the list of all breeds.</returns>
    Task<GetAllBreedsResponseDto> GetAllBreedsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously retrieves the breed with the specified unique identifier.
    /// </summary>
    /// <param name="breedId">The unique identifier of the breed to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Breed"/> with the
    /// specified identifier, or <see langword="null"/> if no such breed exists.</returns>
    Task<Breed> GetBreedByIdAsync(Guid breedId, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously adds a new breed to the specified species.
    /// </summary>
    /// <param name="specieId">The unique identifier of the species to which the new breed will be added.</param>
    /// <param name="name">The name of the breed to add. Cannot be null or empty.</param>
    /// <param name="description">An optional description of the breed. May be null if no description is provided.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the newly created breed.</returns>
    Task<Breed> AddBreedAsync(Guid specieId, string name, string? description, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously updates the specified breed with new name and description values.
    /// </summary>
    /// <param name="breedId">The unique identifier of the breed to update.</param>
    /// <param name="newName">The new name to assign to the breed. If null, the name will not be changed.</param>
    /// <param name="newDescription">The new description to assign to the breed. If null, the description will not be changed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="BreedWithSpecieDto"/>
    /// representing the updated breed and its associated species.</returns>
    Task<BreedWithSpecieDto> UpdateBreedAsync(Guid breedId, string? newName, string? newDescription, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes the breed identified by the specified ID.
    /// </summary>
    /// <param name="breedId">The unique identifier of the breed to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteBreedAsync(Guid breedId, CancellationToken cancellationToken = default);
}

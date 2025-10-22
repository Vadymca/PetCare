namespace PetCare.Infrastructure.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Provides operations for managing animal species, including retrieval, creation, updating, and deletion.
/// </summary>
/// <remarks>This service encapsulates business logic for working with species entities and interacts with the
/// underlying repository to perform data access. All methods are asynchronous and support cancellation via a <see
/// cref="CancellationToken"/>. Instances of this class are intended to be used as a singleton or scoped service in
/// dependency injection scenarios.</remarks>
public sealed class SpecieService : ISpecieService
{
    private readonly ISpeciesRepository specieRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecieService"/> class using the specified repository for Specie entities.
    /// </summary>
    /// <param name="specieRepository">The repository instance used to access and manage Specie entities. Cannot be null.</param>
    /// <param name="mapper">The AutoMapper instance used for object mapping. Cannot be null.</param>
    public SpecieService(
        ISpeciesRepository specieRepository,
        IMapper mapper)
    {
        this.specieRepository = specieRepository ?? throw new ArgumentNullException(nameof(specieRepository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Asynchronously retrieves all available species from the data source.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list containing all species. The list will be empty if no species are found.</returns>
    public async Task<IReadOnlyList<Specie>> GetAllSpeciesAsync(CancellationToken cancellationToken = default)
    {
        return await this.specieRepository.GetAllAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves a species by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the species to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the species with the specified
    /// identifier.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if a species with the specified identifier does not exist.</exception>
    public async Task<Specie> GetSpeciesByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var specie = await this.specieRepository.GetByIdAsync(id, cancellationToken);
        if (specie == null)
        {
            throw new KeyNotFoundException($"Вид тварини з Id '{id}' не знайдено.");
        }

        return specie;
    }

    /// <summary>
    /// Asynchronously retrieves a read-only list of breeds associated with the specified species.
    /// </summary>
    /// <param name="specieId">The unique identifier of the species for which to retrieve breeds.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of breeds for the
    /// specified species. If no breeds are found, the list will be empty.</returns>
    public async Task<IReadOnlyList<Breed>> GetBreedsAsync(Guid specieId, CancellationToken cancellationToken = default)
    {
        return await this.specieRepository.GetBreedsAsync(specieId, cancellationToken);
    }

    /// <summary>
    /// Creates a new species with the specified name and adds it to the repository asynchronously.
    /// </summary>
    /// <param name="name">The name of the species to create. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the newly created species.</returns>
    public async Task<Specie> CreateSpeciesAsync(string name, CancellationToken cancellationToken = default)
    {
        var exists = await this.specieRepository.ExistsByNameAsync(name, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException($"Вид з назвою '{name}' вже існує.");
        }

        var specie = Specie.Create(name);

        await this.specieRepository.AddAsync(specie, cancellationToken);

        return specie;
    }

    /// <summary>
    /// Asynchronously updates the name of an existing species identified by the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the species to update.</param>
    /// <param name="newName">The new name to assign to the species. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated species.</returns>
    public async Task<Specie> UpdateSpeciesAsync(Guid id, string newName, CancellationToken cancellationToken = default)
    {
        var specie = await this.GetSpeciesByIdAsync(id, cancellationToken);
        specie.Rename(newName);
        await this.specieRepository.UpdateAsync(specie, cancellationToken);
        return specie;
    }

    /// <summary>
    /// Deletes the species with the specified identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the species to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    public async Task DeleteSpeciesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var specie = await this.GetSpeciesByIdAsync(id, cancellationToken);
        await this.specieRepository.DeleteAsync(specie, cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves all available breeds and returns them in a response data transfer object.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation before completion.</param>
    /// <returns>A <see cref="GetAllBreedsResponseDto"/> containing a read-only list of breed data and the total count of breeds.
    /// The list will be empty if no breeds are available.</returns>
    public async Task<GetAllBreedsResponseDto> GetAllBreedsAsync(CancellationToken cancellationToken)
    {
        var breeds = await this.specieRepository.GetAllBreedsAsync(cancellationToken);
        var breedDtos = this.mapper.Map<IReadOnlyList<BreedWithSpecieDto>>(breeds);
        return new GetAllBreedsResponseDto(breedDtos, breedDtos.Count);
    }

    /// <summary>
    /// Asynchronously retrieves a breed by its unique identifier.
    /// </summary>
    /// <param name="breedId">The unique identifier of the breed to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the breed with the specified
    /// identifier, or null if no such breed exists.</returns>
    public async Task<Breed> GetBreedByIdAsync(Guid breedId, CancellationToken cancellationToken)
    {
        return await this.specieRepository.GetBreedByIdAsync(breedId, cancellationToken);
    }

    /// <summary>
    /// Asynchronously adds a new breed to the specified species.
    /// </summary>
    /// <param name="specieId">The unique identifier of the species to which the breed will be added.</param>
    /// <param name="name">The name of the breed to add. Cannot be null or empty.</param>
    /// <param name="description">An optional description of the breed. Can be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the newly created breed.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if a species with the specified specieId does not exist.</exception>
    public async Task<Breed> AddBreedAsync(Guid specieId, string name, string? description, CancellationToken cancellationToken)
    {
        return await this.specieRepository.AddBreedAsync(specieId, name, description, cancellationToken);
    }

    /// <summary>
    /// Updates the name and description of the specified breed and returns the updated breed along with its associated
    /// species information.
    /// </summary>
    /// <param name="breedId">The unique identifier of the breed to update.</param>
    /// <param name="newName">The new name to assign to the breed. If null, the name remains unchanged.</param>
    /// <param name="newDescription">The new description to assign to the breed. If null, the description remains unchanged.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="BreedWithSpecieDto"/> containing the updated breed and its associated species information.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if a breed with the specified <paramref name="breedId"/> does not exist.</exception>
    public async Task<BreedWithSpecieDto> UpdateBreedAsync(Guid breedId, string? newName, string? newDescription, CancellationToken cancellationToken)
    {
        var specie = await this.specieRepository.GetSpecieWithBreedAsync(breedId, cancellationToken)
            ?? throw new KeyNotFoundException($"Порода з Id '{breedId}' не знайдена.");

        var breed = specie.Breeds.First(b => b.Id == breedId);
        breed.Update(newName, newDescription);

        await this.specieRepository.UpdateAsync(specie, cancellationToken);

        return this.mapper.Map<BreedWithSpecieDto>(breed);
    }

    /// <summary>
    /// Asynchronously deletes the breed with the specified identifier from its associated species.
    /// </summary>
    /// <param name="breedId">The unique identifier of the breed to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if a breed with the specified <paramref name="breedId"/> does not exist.</exception>
    public async Task DeleteBreedAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        var specie = await this.specieRepository.GetSpecieWithBreedAsync(breedId, cancellationToken)
            ?? throw new KeyNotFoundException($"Порода з Id '{breedId}' не знайдена.");

        var breed = specie.Breeds.First(b => b.Id == breedId);

        // Видаляємо породу з агрегату
        specie.RemoveBreed(breed.Id);

        // Оновлюємо агрегат у БД
        await this.specieRepository.UpdateAsync(specie, cancellationToken);
    }
}

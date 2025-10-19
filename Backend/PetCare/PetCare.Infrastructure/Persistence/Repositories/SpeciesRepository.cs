namespace PetCare.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Specifications.Specie;
using PetCare.Infrastructure.Persistence;

/// <summary>
/// Repository implementation for managing <see cref="Specie"/> entities.
/// </summary>
public class SpeciesRepository : GenericRepository<Specie>, ISpeciesRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpeciesRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SpeciesRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<Specie?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await this.FindAsync(new SpecieByNameSpecification(name), cancellationToken)
               .ContinueWith(t => t.Result.FirstOrDefault(), cancellationToken);

    /// <summary>
    /// Retrieves all breeds for a given species ID.
    /// </summary>
    /// <param name="specieId">The ID of the species.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of breeds for the species.</returns>
    public async Task<IReadOnlyList<Breed>> GetBreedsAsync(Guid specieId, CancellationToken cancellationToken = default)
    {
        var specie = await this.Context.Set<Specie>()
            .AsNoTracking()
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == specieId, cancellationToken)
            ?? throw new KeyNotFoundException($"Вид з Id '{specieId}' не знайдено.");

        return specie.Breeds.ToList().AsReadOnly();
    }

    /// <summary>
    /// Asynchronously retrieves a read-only list of all dog breeds from the data store, ordered by breed name.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of <see cref="Breed"/> objects representing all breeds in the data store. The list will be
    /// empty if no breeds are found.</returns>
    public async Task<IReadOnlyList<Breed>> GetAllBreedsAsync(CancellationToken cancellationToken)
    {
        return await this.Context.Set<Breed>()
            .AsNoTracking()
            .Include(b => b.Specie)
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves a breed by its unique identifier.
    /// </summary>
    /// <param name="breedId">The unique identifier of the breed to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the breed with the specified
    /// identifier.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if a breed with the specified identifier is not found.</exception>
    public async Task<Breed> GetBreedByIdAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        var breed = await this.Context.Set<Breed>()
            .AsNoTracking()
            .Include(b => b.Specie)
            .FirstOrDefaultAsync(b => b.Id == breedId, cancellationToken);

        if (breed == null)
        {
            throw new KeyNotFoundException($"Порода з Id '{breedId}' не знайдено.");
        }

        return breed;
    }

    /// <summary>
    /// Asynchronously retrieves the species that contains the specified breed.
    /// </summary>
    /// <remarks>The returned species includes its associated breeds. This method queries the data source and
    /// may incur a database call.</remarks>
    /// <param name="breedId">The unique identifier of the breed to search for within species.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the species that includes the
    /// specified breed, or <see langword="null"/> if no such species is found.</returns>
    public async Task<Specie?> GetSpecieWithBreedAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<Specie>()
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Breeds.Any(b => b.Id == breedId), cancellationToken);
    }
}

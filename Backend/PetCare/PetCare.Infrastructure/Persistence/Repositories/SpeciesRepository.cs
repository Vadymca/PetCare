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

    /// <summary>
    /// Asynchronously adds a new breed to the specified species.
    /// </summary>
    /// <param name="specieId">The unique identifier of the species to which the breed will be added.</param>
    /// <param name="name">The name of the breed to add. Cannot be null or empty.</param>
    /// <param name="description">An optional description of the breed. Can be null if no description is provided.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the newly created breed.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if a species with the specified <paramref name="specieId"/> does not exist.</exception>
    public async Task<Breed> AddBreedAsync(Guid specieId, string name, string? description, CancellationToken cancellationToken)
    {
        // Завантажуємо Specie разом з колекцією Breeds
        var specie = await this.Context.Set<Specie>()
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == specieId, cancellationToken)
            ?? throw new KeyNotFoundException($"Вид з Id '{specieId}' не знайдено.");

        // Перевірка, чи існує вже така порода (без урахування регістру)
        var isDuplicate = specie.Breeds
            .Any(b => string.Equals(b.Name.Value, name, StringComparison.OrdinalIgnoreCase));

        if (isDuplicate)
        {
            throw new InvalidOperationException($"Порода з назвою '{name}' вже існує для цього виду.");
        }

        // Створюємо нову породу
        var breed = Breed.Create(specieId, name, description);

        // Додаємо безпосередньо у контекст EF
        this.Context.Set<Breed>().Add(breed);

        await this.Context.SaveChangesAsync(cancellationToken);
        return breed;
    }

    /// <summary>
    /// Determines whether a species with the specified name exists in the data store.
    /// </summary>
    /// <param name="name">The name of the species to search for. Comparison is case-insensitive.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if a species with
    /// the specified name exists; otherwise, <see langword="false"/>.</returns>
    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
    {
        var allNames = await this.Context.Species
            .Select(s => s.Name.Value)
            .ToListAsync(cancellationToken);

        return allNames.Any(n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase));
    }
}

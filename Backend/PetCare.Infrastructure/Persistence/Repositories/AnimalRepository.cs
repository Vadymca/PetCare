namespace PetCare.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
<<<<<<< HEAD
using PetCare.Infrastructure.Persistence;
=======
using PetCare.Domain.Specifications.Animal;
using PetCare.Infrastructure.Persistence;
using System.Threading;
>>>>>>> 5e60776 (Implementation of repositories for aggregates)

/// <summary>
/// Repository for managing <see cref="Animal"/> aggregate.
/// </summary>
public class AnimalRepository : GenericRepository<Animal>, IAnimalRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public AnimalRepository(AppDbContext context)
        : base(context)
    {
    }

<<<<<<< HEAD
    /// <summary>
    /// Retrieves an animal by its slug.
    /// </summary>
    /// <param name="slug">The slug identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The animal matching the slug or null if not found.</returns>
    public async Task<Animal?> GetBySlugAsync(
        string slug, CancellationToken cancellationToken = default)
    {
        return await this.Context.Animals
=======
    /// <inheritdoc />
    public Task<IReadOnlyList<Animal>> GetByShelterIdAsync(Guid shelterId, CancellationToken cancellationToken = default)
        => this.FindAsync(new AnimalsByShelterSpecification(shelterId), cancellationToken);

    /// <inheritdoc />
    public Task<IReadOnlyList<Animal>> GetByBreedIdAsync(Guid breedId, CancellationToken cancellationToken = default)
        => this.FindAsync(new AnimalsByBreedSpecification(breedId), cancellationToken);

    /// <inheritdoc />
    public Task<IReadOnlyList<Animal>> GetAvailableForAdoptionAsync(CancellationToken cancellationToken = default)
        => this.FindAsync(new AvailableAnimalsSpecification(), cancellationToken);

    /// <inheritdoc />
    public async Task<Animal?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Slug не може бути порожнім.", nameof(slug));
        }

        return await this.Context.Set<Animal>()
            .AsNoTracking()
            .Include(a => a.Breed)
            .Include(a => a.Shelter)
            .Include(a => a.AdoptionApplications)
            .Include(a => a.Tags)
            .Include(a => a.SuccessStories)
            .Include(a => a.Subscribers)
>>>>>>> 5e60776 (Implementation of repositories for aggregates)
            .FirstOrDefaultAsync(a => a.Slug.Value == slug, cancellationToken);
    }
}

namespace PetCare.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Specifications.Animal;
using PetCare.Infrastructure.Persistence;
using System.Threading;

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
            .FirstOrDefaultAsync(a => a.Slug.Value == slug, cancellationToken);
    }
}

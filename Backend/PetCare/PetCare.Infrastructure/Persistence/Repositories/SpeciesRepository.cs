namespace PetCare.Infrastructure.Persistence.Repositories;

using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
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
}

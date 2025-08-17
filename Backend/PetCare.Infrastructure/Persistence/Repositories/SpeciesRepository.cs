namespace PetCare.Infrastructure.Persistence.Repositories;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
=======
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Specifications.Specie;
>>>>>>> 5e60776 (Implementation of repositories for aggregates)
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
<<<<<<< HEAD
    public async Task<Specie?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await this.Context.Species
            .FirstOrDefaultAsync(s => s.Name.Value == name, cancellationToken);
    }
=======
    public Task<Specie?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => this.FindAsync(new SpecieByNameSpecification(name), cancellationToken)
               .ContinueWith(t => t.Result.FirstOrDefault(), cancellationToken);
>>>>>>> 5e60776 (Implementation of repositories for aggregates)
}

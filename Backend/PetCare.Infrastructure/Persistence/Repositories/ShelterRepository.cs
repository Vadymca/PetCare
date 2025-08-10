namespace PetCare.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Infrastructure.Persistence;

/// <summary>
/// Repository implementation for managing <see cref="Shelter"/> entities.
/// </summary>
public class ShelterRepository : GenericRepository<Shelter>, IShelterRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShelterRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ShelterRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<Shelter?> GetBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await this.Context.Shelters
            .FirstOrDefaultAsync(s => s.Slug.Value == slug, cancellationToken);
    }
}

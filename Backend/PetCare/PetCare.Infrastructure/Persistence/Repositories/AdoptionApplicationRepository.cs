namespace PetCare.Infrastructure.Persistence.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Specifications.AdoptionApplication;

/// <summary>
/// Repository implementation for managing <see cref="AdoptionApplication"/> entities.
/// </summary>
public class AdoptionApplicationRepository : GenericRepository<AdoptionApplication>, IAdoptionApplicationRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AdoptionApplicationRepository"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public AdoptionApplicationRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<AdoptionApplication>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await this.FindAsync(new AdoptionApplicationsByUserSpecification(userId), cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<AdoptionApplication>> GetByAnimalIdAsync(Guid animalId, CancellationToken cancellationToken = default)
        => await this.FindAsync(new AdoptionApplicationsByAnimalSpecification(animalId), cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<AdoptionApplication>> GetPendingApplicationsAsync(CancellationToken cancellationToken = default)
        => await this.FindAsync(new PendingAdoptionApplicationsSpecification(), cancellationToken);
}

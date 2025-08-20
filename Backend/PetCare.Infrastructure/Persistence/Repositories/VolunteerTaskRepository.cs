namespace PetCare.Infrastructure.Persistence.Repositories;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Specifications.VolunteerTask;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Repository for managing volunteer tasks.
/// </summary>
public class VolunteerTaskRepository : GenericRepository<VolunteerTask>, IVolunteerTaskRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VolunteerTaskRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public VolunteerTaskRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<VolunteerTask>> GetByShelterIdAsync(Guid shelterId, CancellationToken cancellationToken = default)
        => await this.FindAsync(new VolunteerTasksByShelterSpecification(shelterId), cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<VolunteerTask>> GetByDateAsync(DateOnly date, CancellationToken cancellationToken = default)
         => await this.FindAsync(new VolunteerTasksByDateSpecification(date), cancellationToken);
}

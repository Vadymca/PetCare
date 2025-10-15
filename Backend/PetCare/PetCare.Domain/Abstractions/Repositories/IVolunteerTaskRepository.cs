namespace PetCare.Domain.Abstractions.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;

/// <summary>
/// Represents a repository interface for accessing volunteer tasks.
/// </summary>
public interface IVolunteerTaskRepository : IRepository<VolunteerTask>
{
    /// <summary>
    /// Retrieves all volunteer tasks for a specific shelter.
    /// </summary>
    /// <param name="shelterId">The unique identifier of the shelter.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of volunteer tasks.</returns>
    Task<IReadOnlyList<VolunteerTask>> GetByShelterIdAsync(
        Guid shelterId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all volunteer tasks scheduled on a specific date.
    /// </summary>
    /// <param name="date">The date of the tasks.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of volunteer tasks.</returns>
    Task<IReadOnlyList<VolunteerTask>> GetByDateAsync(
        DateOnly date,
        CancellationToken cancellationToken = default);
}

namespace PetCare.Domain.Abstractions.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;

/// <summary>
/// Represents a repository interface for managing <see cref="AdoptionApplication"/> entities.
/// </summary>
public interface IAdoptionApplicationRepository : IRepository<AdoptionApplication>
{
    /// <summary>
    /// Retrieves all adoption applications for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of adoption applications.</returns>
    Task<IReadOnlyList<AdoptionApplication>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all adoption applications for a specific animal.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of adoption applications.</returns>
    Task<IReadOnlyList<AdoptionApplication>> GetByAnimalIdAsync(Guid animalId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all pending adoption applications.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of pending adoption applications.</returns>
    Task<IReadOnlyList<AdoptionApplication>> GetPendingApplicationsAsync(CancellationToken cancellationToken = default);
}

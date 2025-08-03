// <copyright file="IBreedRepository.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure.Persistence.Repositories;
using PetCare.Domain.Entities;

/// <summary>
/// Represents a repository interface for accessing breed entities.
/// </summary>
public interface IBreedRepository : IRepository<Breed>
{
    /// <summary>
    /// Retrieves all breeds for a given species identifier.
    /// </summary>
    /// <param name="speciesId">The identifier of the species.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a read-only list of breeds.
    /// </returns>
    Task<IReadOnlyList<Breed>> GetBySpeciesIdAsync(
        Guid speciesId, CancellationToken cancellationToken = default);
}

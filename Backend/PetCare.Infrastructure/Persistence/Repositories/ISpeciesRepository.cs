// <copyright file="ISpeciesRepository.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure.Persistence.Repositories;
using PetCare.Domain.Entities;

/// <summary>
/// Repository interface for accessing species entities.
/// </summary>
public interface ISpeciesRepository : IRepository<Specie>
{
    /// <summary>
    /// Retrieves a species entity by its name.
    /// </summary>
    /// <param name="name">The name of the species.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the species if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Specie?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

// <copyright file="IAnimalRepository.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure.Persistence.Repositories;
using PetCare.Domain.Aggregates;

/// <summary>
/// Represents a repository interface for accessing animal entities.
/// </summary>
public interface IAnimalRepository : IRepository<Animal>
{
    /// <summary>
    /// Retrieves an animal by its unique slug.
    /// </summary>
    /// <param name="slug">The slug of the animal.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the animal if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Animal?> GetBySlugAsync(
        string slug, CancellationToken cancellationToken = default);
}

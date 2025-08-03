// <copyright file="AnimalRepository.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Aggregates;

/// <summary>
/// Repository for managing <see cref="Animal"/> aggregate.
/// </summary>
public class AnimalRepository : GenericRepository<Animal>, IAnimalRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public AnimalRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <summary>
    /// Retrieves an animal by its slug.
    /// </summary>
    /// <param name="slug">The slug identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The animal matching the slug or null if not found.</returns>
    public async Task<Animal?> GetBySlugAsync(
        string slug, CancellationToken cancellationToken = default)
    {
        return await this.Context.Animals
            .FirstOrDefaultAsync(a => a.Slug.Value == slug, cancellationToken);
    }
}

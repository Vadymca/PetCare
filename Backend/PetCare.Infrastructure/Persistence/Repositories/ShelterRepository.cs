// <copyright file="ShelterRepository.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Aggregates;

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

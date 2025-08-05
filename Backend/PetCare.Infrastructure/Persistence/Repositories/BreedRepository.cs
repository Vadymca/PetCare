// <copyright file="BreedRepository.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Entities;
using PetCare.Domain.Interfaces;
using PetCare.Infrastructure.Persistence;

/// <summary>
/// Repository for managing <see cref="Breed"/> entities.
/// </summary>
public class BreedRepository : GenericRepository<Breed>, IBreedRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BreedRepository"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public BreedRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <summary>
    /// Retrieves all breeds that belong to the specified species.
    /// </summary>
    /// <param name="speciesId">The ID of the species.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A read-only list of breeds that match the species ID.</returns>
    public async Task<IReadOnlyList<Breed>> GetBySpeciesIdAsync(
        Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        return await this.Context.Breeds
            .Where(b => b.SpeciesId == speciesId)
            .ToListAsync(cancellationToken);
    }
}

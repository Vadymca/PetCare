// <copyright file="SpeciesRepository.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Entities;

/// <summary>
/// Repository implementation for managing <see cref="Specie"/> entities.
/// </summary>
public class SpeciesRepository : GenericRepository<Specie>, ISpeciesRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpeciesRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SpeciesRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<Specie?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await this.Context.Species
            .FirstOrDefaultAsync(s => s.Name.Value == name, cancellationToken);
    }
}

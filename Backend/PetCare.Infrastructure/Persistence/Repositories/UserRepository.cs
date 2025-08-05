// <copyright file="UserRepository.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Interfaces;
using PetCare.Infrastructure.Persistence;

/// <summary>
/// Repository implementation for managing <see cref="User"/> entities.
/// </summary>
public class UserRepository : GenericRepository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRepository(AppDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<User?> GetByEmailAsync(
        string email, CancellationToken cancellationToken = default)
    {
        return await this.Context.Users
            .FirstOrDefaultAsync(
                u =>
            u.Email.Value == email, cancellationToken);
    }
}

// <copyright file="GenericRepository.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// A generic repository for performing basic CRUD operations.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public class GenericRepository<T> : IRepository<T>
    where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GenericRepository(AppDbContext context)
    {
        this.Context = context;
    }

    /// <summary>
    /// Gets the database context used by the repository.
    /// </summary>
    protected AppDbContext Context { get; }

    /// <summary>
    /// Adds a new entity to the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The added entity.</returns>
    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        this.Context.Set<T>().Add(entity);
        await this.Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The updated entity.</returns>
    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        this.Context.Set<T>().Update(entity);
        await this.Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <summary>
    /// Deletes an entity from the database.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        this.Context.Set<T>().Remove(entity);
        await this.Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
    }

    /// <summary>
    /// Retrieves all entities of the specified type.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of all entities.</returns>
    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<T>().ToListAsync(cancellationToken);
    }
}

namespace PetCare.Domain.Abstractions.Repositories;

using PetCare.Domain.Specifications;

/// <summary>
/// Represents a generic repository interface for performing CRUD operations on entities.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IRepository<T>
    where T : class
{
    /// <summary>
    /// Adds a new entity to the data store.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that returns the added entity.</returns>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity in the data store.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that returns the updated entity.</returns>
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the specified entity from the data store.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task that returns the entity if found; otherwise, <c>null</c>.
    /// </returns>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all entities of the specified type.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that returns a read-only list of all entities.</returns>
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the number of entities in the repository.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The total number of entities.</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds entities based on a specification.
    /// </summary>
    /// <param name="specification">The specification to filter entities.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of entities satisfying the specification.</returns>
    Task<IReadOnlyList<T>> FindAsync(Specification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Detaches the specified entity from the current context, so that changes to the entity are no longer tracked.
    /// </summary>
    /// <remarks>After detaching, modifications to the entity will not be persisted by the context. Use this
    /// method when you want to stop tracking changes for an entity instance.</remarks>
    /// <param name="entity">The entity to detach from the context. Cannot be null.</param>
    void Detach(T entity);
}

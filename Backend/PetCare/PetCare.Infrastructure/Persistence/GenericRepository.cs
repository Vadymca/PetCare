namespace PetCare.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Specifications;

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
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
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
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        this.Context.Set<T>().Add(entity);
        await this.Context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the entity has no primary key or does not exist.</exception>
    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Сутність не може бути null.");
        }

        // Отримуємо метадані первинного ключа сутності
        var key = this.Context.Model.FindEntityType(typeof(T))?.FindPrimaryKey();

        if (key == null || key.Properties.Count != 1)
        {
            throw new InvalidOperationException("Сутність повинна мати один первинний ключ.");
        }

        var keyProperty = key.Properties[0];

        // Отримуємо значення ключа
        var keyValue = typeof(T).GetProperty(keyProperty.Name)?.GetValue(entity);
        if (keyValue == null)
        {
            throw new InvalidOperationException("Значення ключа не може бути null.");
        }

        // Перевіряємо чи існує сутність у БД
        var exists = await this.Context.Set<T>()
            .AsNoTracking()
            .AnyAsync(e => EF.Property<object>(e, keyProperty.Name)!.Equals(keyValue), cancellationToken);

        if (!exists)
        {
            throw new InvalidOperationException($"Сутність типу {typeof(T).Name} з ключем {keyValue} не знайдено.");
        }

        // Оновлюємо сутність
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
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var key = this.Context.Model.FindEntityType(typeof(T))?.FindPrimaryKey();

        if (key == null)
        {
            throw new InvalidOperationException($"Сутність {typeof(T).Name} не має визначеного первинного ключа.");
        }

        if (key.Properties.Count != 1)
        {
            throw new NotSupportedException("У цій реалізації сховища не підтримуються складені ключі.");
        }

        var keyProperty = key.Properties[0];
        var keyValue = typeof(T).GetProperty(keyProperty.Name)?.GetValue(entity);

        if (keyValue == null)
        {
            throw new InvalidOperationException("Значення ключа не може бути нульовим.");
        }

        var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");
        var propertyAccess = System.Linq.Expressions.Expression.Property(parameter, keyProperty.Name);
        var constant = System.Linq.Expressions.Expression.Constant(keyValue);
        var equal = System.Linq.Expressions.Expression.Equal(propertyAccess, constant);
        var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(equal, parameter);

        await this.Context.Set<T>().Where(lambda).ExecuteDeleteAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<T>()
            .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all entities of the specified type.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of all entities.</returns>
    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<T>()
           .AsNoTracking()
           .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Counts the number of entities in the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The total number of entities.</returns>
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await this.Context.Set<T>().CountAsync(cancellationToken);
    }

    /// <summary>
    /// Finds entities based on a specification.
    /// </summary>
    /// <param name="specification">The specification to filter entities.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of entities satisfying the specification.</returns>
    public async Task<IReadOnlyList<T>> FindAsync(
        Specification<T> specification,
        CancellationToken cancellationToken = default)
    {
        if (specification == null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        return await this.Context.Set<T>()
            .AsNoTracking()
            .Where(specification.ToExpression())
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Detaches the specified entity from the context, so that changes to the entity are not tracked or persisted.
    /// </summary>
    /// <remarks>Use this method to stop tracking changes for an entity, typically when you want to prevent
    /// updates or deletes from being applied to the database. After detachment, the entity will not be included in save
    /// operations. This method is commonly used in scenarios where entities are reused or when explicit control over
    /// tracking is required.</remarks>
    /// <param name="entity">The entity to detach from the context. Must not be null.</param>
    public void Detach(T entity)
    {
        var entry = this.Context.Entry(entity);
        if (entry != null)
        {
            entry.State = EntityState.Detached;
        }
    }
}

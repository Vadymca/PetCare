namespace PetCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Repositories;

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
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The updated entity.</returns>
    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var key = this.Context.Model.FindEntityType(typeof(T))?.FindPrimaryKey();

        if (key == null || key.Properties.Count != 1)
        {
            throw new InvalidOperationException("Сутність повинна мати єдиний первинний ключ");
        }

        var keyProperty = key.Properties[0];

        var keyValue = typeof(T).GetProperty(keyProperty.Name)?.GetValue(entity);

        if (keyValue == null)
        {
            throw new InvalidOperationException("Значення ключа не може бути нульовимl");
        }

        var exists = await this.Context.Set<T>()
            .AsNoTracking()
            .AnyAsync(e => EF.Property<object>(e, keyProperty.Name)!.Equals(keyValue), cancellationToken);

        if (!exists)
        {
            throw new InvalidOperationException($"Сутність типу {typeof(T).Name} з ключем {keyValue} не знайдено.");
        }

        this.Context.Set<T>().Attach(entity);
        this.Context.Entry(entity).State = EntityState.Modified;

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
            .AsNoTracking()
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
}

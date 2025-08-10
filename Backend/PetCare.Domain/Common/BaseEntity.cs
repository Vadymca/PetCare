namespace PetCare.Domain.Common;

/// <summary>
/// Represents a base entity with a unique identifier.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEntity"/> class with a unique identifier.
    /// </summary>
    protected BaseEntity() => this.Id = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    public Guid Id { get; protected set; }
}

namespace PetCare.Domain.Common;
using PetCare.Domain.Events;

/// <summary>
/// Represents the base class for aggregate roots that can raise domain events.
/// Inherits from <see cref="BaseEntity"/> to include common entity properties.
/// </summary>
public abstract class AggregateRoot : BaseEntity
{
    private readonly List<DomainEvent> domainEvents = new();

    /// <summary>
    /// Gets the read-only collection of domain events raised by the aggregate.
    /// </summary>
    public IReadOnlyCollection<DomainEvent> DomainEvents => this.domainEvents.AsReadOnly();

    /// <summary>
    /// Clears all domain events from the aggregate.
    /// </summary>
    public void ClearDomainEvents()
    {
        this.domainEvents.Clear();
    }

    /// <summary>
    /// Adds a domain event to the aggregate's event collection.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        this.domainEvents.Add(domainEvent);
    }
}
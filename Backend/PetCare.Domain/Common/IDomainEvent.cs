using MediatR;

namespace PetCare.Domain.Common
{
    /// <summary>
    /// Marker interface for domain events
    /// </summary>
    public interface IDomainEvent : INotification
    {
        /// <summary>
        /// Unique identifier for the event
        /// </summary>
        Guid EventId { get; }
        
        /// <summary>
        /// When the event occurred
        /// </summary>
        DateTime OccurredOn { get; }
        
        /// <summary>
        /// The aggregate that raised the event
        /// </summary>
        Guid AggregateId { get; }
        
        /// <summary>
        /// Version of the aggregate when the event was raised
        /// </summary>
        int AggregateVersion { get; }
    }
}

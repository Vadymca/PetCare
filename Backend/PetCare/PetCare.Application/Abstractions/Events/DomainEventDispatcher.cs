namespace PetCare.Application.Abstractions.Events;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Domain.Abstractions.Events;

/// <summary>
/// Default implementation of the domain event dispatcher using MediatR.
/// </summary>
public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher publisher;
    private readonly ILogger<DomainEventDispatcher> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEventDispatcher"/> class with the specified event publisher and logger.
    /// </summary>
    /// <param name="publisher">The event publisher used to dispatch domain events. Cannot be null.</param>
    /// <param name="logger">The logger used for logging domain event dispatching operations. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if publisher or logger is null.</exception>
    public DomainEventDispatcher(IPublisher publisher, ILogger<DomainEventDispatcher> logger)
    {
        this.publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Asynchronously dispatches a collection of domain events to the configured event publisher.
    /// </summary>
    /// <remarks>The events are published in the order they appear in the collection. If the operation is
    /// canceled via the cancellation token, not all events may be dispatched.</remarks>
    /// <param name="events">The collection of domain events to dispatch. Cannot be null. Each event in the collection will be published in
    /// sequence.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the dispatch operation.</param>
    /// <returns>A task that represents the asynchronous dispatch operation.</returns>
    public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Dispatching {EventCount} domain events", events.Count());
        foreach (var domainEvent in events)
        {
            this.logger.LogInformation("Publishing event: {EventType} with data: {@Event}", domainEvent.GetType().Name, domainEvent);
            await this.publisher.Publish(domainEvent, cancellationToken);
        }
    }
}
namespace PetCare.Application.Abstractions.Events;

using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Domain.Abstractions.Events;
using PetCare.Domain.Events;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Default implementation of the domain event dispatcher using MediatR.
/// </summary>
public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher publisher;
    private readonly ILogger<DomainEventDispatcher> logger;

    public DomainEventDispatcher(IPublisher publisher, ILogger<DomainEventDispatcher> logger)
    {
        this.publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

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
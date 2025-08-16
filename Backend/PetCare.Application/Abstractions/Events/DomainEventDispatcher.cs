namespace PetCare.Application.Abstractions.Events;

using MediatR;
using PetCare.Domain.Abstractions.Events;
using PetCare.Domain.Events;

/// <summary>
/// Default implementation of the domain event dispatcher using MediatR.
/// </summary>
public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator mediator;

    public DomainEventDispatcher(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task DispatchAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in events)
        {
            await this.mediator.Publish(domainEvent, cancellationToken).ConfigureAwait(false);
        }
    }
}

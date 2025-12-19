namespace PetCare.Application.EventHandlers.AdoptionApplications;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles AdoptionApplicationUpdatedEvent.
/// </summary>
public sealed class AdoptionApplicationUpdatedEventHandler : INotificationHandler<AdoptionApplicationUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AdoptionApplicationUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}

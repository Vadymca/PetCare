namespace PetCare.Application.EventHandlers.AdoptionApplications;

using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles AdoptionApplicationCompletedEvent.
/// </summary>
public sealed class AdoptionApplicationCompletedEventHandler : INotificationHandler<AdoptionApplicationCompletedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AdoptionApplicationCompletedEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}

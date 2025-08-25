namespace PetCare.Application.EventHandlers.AdoptionApplications;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AdoptionApplicationNotesUpdatedEvent.
/// </summary>
public sealed class AdoptionApplicationNotesUpdatedEventHandler : INotificationHandler<AdoptionApplicationNotesUpdatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AdoptionApplicationNotesUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}

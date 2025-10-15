namespace PetCare.Application.EventHandlers.AdoptionApplications;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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

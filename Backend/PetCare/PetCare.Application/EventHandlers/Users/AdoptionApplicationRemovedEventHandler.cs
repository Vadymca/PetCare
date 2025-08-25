namespace PetCare.Application.EventHandlers.Users;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AdoptionApplicationRemovedEvent.
/// </summary>
public sealed class AdoptionApplicationRemovedEventHandler : INotificationHandler<AdoptionApplicationRemovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AdoptionApplicationRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Логіка
        await Task.CompletedTask;
    }
}

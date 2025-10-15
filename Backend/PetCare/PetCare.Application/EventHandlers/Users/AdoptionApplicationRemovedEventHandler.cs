namespace PetCare.Application.EventHandlers.Users;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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

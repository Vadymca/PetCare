namespace PetCare.Application.EventHandlers.AdoptionApplications;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AdoptionApplicationApprovedEvent.
/// </summary>
public sealed class AdoptionApplicationApprovedEventHandler : INotificationHandler<AdoptionApplicationApprovedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AdoptionApplicationApprovedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}

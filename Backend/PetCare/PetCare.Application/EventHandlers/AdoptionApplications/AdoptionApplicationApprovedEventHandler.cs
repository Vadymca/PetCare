namespace PetCare.Application.EventHandlers.AdoptionApplications;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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

namespace PetCare.Application.EventHandlers.AdoptionApplications;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

/// <summary>
/// Handles AdoptionApplicationRejectedEvent.
/// </summary>
public sealed class AdoptionApplicationRejectedEventHandler : INotificationHandler<AdoptionApplicationRejectedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AdoptionApplicationRejectedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}

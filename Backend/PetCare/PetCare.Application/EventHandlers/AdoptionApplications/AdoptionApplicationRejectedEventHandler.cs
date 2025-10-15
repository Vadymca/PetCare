namespace PetCare.Application.EventHandlers.AdoptionApplications;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

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

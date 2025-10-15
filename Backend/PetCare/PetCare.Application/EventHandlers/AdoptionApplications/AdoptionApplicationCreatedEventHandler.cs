namespace PetCare.Application.EventHandlers.AdoptionApplications;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles AdoptionApplicationCreatedEvent.
/// </summary>
public sealed class AdoptionApplicationCreatedEventHandler : INotificationHandler<AdoptionApplicationCreatedEvent>
{
    /// <inheritdoc/>
    public async Task Handle(AdoptionApplicationCreatedEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}

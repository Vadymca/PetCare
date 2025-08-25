namespace PetCare.Application.EventHandlers.AdoptionApplications;

using MediatR;
using PetCare.Domain.Events;
using System.Threading.Tasks;

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

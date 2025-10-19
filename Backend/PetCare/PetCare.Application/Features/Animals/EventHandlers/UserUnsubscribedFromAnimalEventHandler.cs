namespace PetCare.Application.Features.Animals.EventHandlers;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles notifications when a user unsubscribes from receiving updates about a specific animal.
/// </summary>
/// <remarks>This handler processes the <see cref="UserUnsubscribedFromAnimalEvent"/> notification, allowing the
/// application to respond to user unsubscription events. It is typically used within a MediatR pipeline to trigger any
/// necessary actions or cleanup when a user opts out of animal-related notifications.</remarks>
public sealed class UserUnsubscribedFromAnimalEventHandler : INotificationHandler<UserUnsubscribedFromAnimalEvent>
{
    /// <inheritdoc/>
    public async Task Handle(UserUnsubscribedFromAnimalEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}

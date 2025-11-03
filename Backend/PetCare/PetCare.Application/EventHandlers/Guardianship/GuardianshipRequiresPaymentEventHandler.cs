namespace PetCare.Application.EventHandlers.Guardianship;

using System.Threading.Tasks;
using MediatR;
using PetCare.Domain.Events;

/// <summary>
/// Handles notifications indicating that a guardianship requires payment.
/// </summary>
/// <remarks>This event handler processes instances of <see cref="GuardianshipRequiresPaymentEvent"/> when
/// they are published. It is typically used within a MediatR pipeline to respond to payment-related events in
/// guardianship workflows.</remarks>
public sealed class GuardianshipRequiresPaymentEventHandler : INotificationHandler<GuardianshipRequiresPaymentEvent>
{
    /// <inheritdoc/>
    public async Task Handle(GuardianshipRequiresPaymentEvent notification, CancellationToken cancellationToken)
    {
        // логіка
        await Task.CompletedTask;
    }
}

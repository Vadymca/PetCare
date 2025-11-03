namespace PetCare.Application.Features.Payments.CancelSubscription;

using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;
using PetCare.Domain.Enums;

/// <summary>
/// Handles the cancellation of payment subscriptions.
/// </summary>
public sealed class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand>
{
    private readonly ISubscriptionService subscriptions;
    private readonly IGuardianshipService guardianships;
    private readonly ILogger<CancelSubscriptionCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancelSubscriptionCommandHandler"/> class with the specified subscription and.
    /// guardianship services and logger.
    /// </summary>
    /// <param name="subscriptions">The service used to manage and process subscription operations.</param>
    /// <param name="guardianships">The service used to validate and manage guardianship relationships relevant to subscription cancellation.</param>
    /// <param name="logger">The logger used to record diagnostic and operational information for this handler.</param>
    public CancelSubscriptionCommandHandler(
        ISubscriptionService subscriptions,
        IGuardianshipService guardianships,
        ILogger<CancelSubscriptionCommandHandler> logger)
    {
        this.subscriptions = subscriptions;
        this.guardianships = guardianships;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var sub = await this.subscriptions.FindByProviderSubscriptionIdAsync(request.ProviderSubscriptionId, cancellationToken);

        if (sub is null)
        {
            this.logger.LogWarning("Subscription {ProviderSubscriptionId} not found.", request.ProviderSubscriptionId);
            throw new KeyNotFoundException($"Підписку з ID '{request.ProviderSubscriptionId}' не знайдено.");
        }

        if (sub.ScopeType == SubscriptionScope.Guardianship && sub.ScopeId is not null)
        {
            this.logger.LogInformation("Cancelling guardianship-linked subscription {ProviderSubscriptionId}.", sub.ProviderSubscriptionId);
            await this.guardianships.CompleteAsync(sub.ScopeId.Value, cancelSubscription: true, cancellationToken);
        }
        else
        {
            this.logger.LogInformation("Cancelling standalone subscription {ProviderSubscriptionId}.", sub.ProviderSubscriptionId);
            await this.subscriptions.CancelAsync(sub.ProviderSubscriptionId, cancellationToken);
        }

        this.logger.LogInformation("Subscription {ProviderSubscriptionId} cancelled successfully.", sub.ProviderSubscriptionId);
    }
}

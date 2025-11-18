namespace PetCare.Application.Features.Payments.Guardianships.DeleteGuardianship;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles deletion of guardianships, including cancellation of active subscriptions.
/// </summary>
public sealed class DeleteGuardianshipCommandHandler
    : IRequestHandler<DeleteGuardianshipCommand, GuardianshipDeletedDto>
{
    private readonly IGuardianshipService guardianshipService;
    private readonly ISubscriptionService subscriptionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteGuardianshipCommandHandler"/> class.
    /// </summary>
    /// <param name="guardianshipService">Service for working with guardianships.</param>
    /// <param name="subscriptionService">Service for managing payment subscriptions.</param>
    public DeleteGuardianshipCommandHandler(
        IGuardianshipService guardianshipService,
        ISubscriptionService subscriptionService)
    {
        this.guardianshipService = guardianshipService ?? throw new ArgumentNullException(nameof(guardianshipService));
        this.subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
    }

    /// <inheritdoc />
    public async Task<GuardianshipDeletedDto> Handle(
        DeleteGuardianshipCommand request,
        CancellationToken cancellationToken)
    {
        // Will throw if not found or not owned by user (ownership may be implemented)
        var (guardianship, subscription) =
            await this.guardianshipService.GetWithSubscriptionAsync(request.GuardianshipId, cancellationToken);

        var hadSubscription = subscription is not null;
        var canceledSubscription = false;

        if (subscription is not null && subscription.IsActive)
        {
            await this.subscriptionService.CancelAsync(subscription.ProviderSubscriptionId!, cancellationToken);
            canceledSubscription = true;
        }

        await this.guardianshipService.DeleteAsync(guardianship.Id, cancellationToken);

        return new GuardianshipDeletedDto(
            guardianship.Id,
            hadSubscription,
            canceledSubscription,
            BuildMessage(hadSubscription, canceledSubscription));
    }

    private static string BuildMessage(bool hadSubscription, bool canceledSubscription)
    {
        if (!hadSubscription)
        {
            return "Опіку успішно видалено. Платіжної підписки не було.";
        }

        if (hadSubscription && canceledSubscription)
        {
            return "Опіку успішно видалено. Платіжну підписку скасовано.";
        }

        if (hadSubscription && !canceledSubscription)
        {
            return "Опіку видалено, але скасувати підписку не вдалося.";
        }

        return "Опіка успішно видалена.";
    }
}

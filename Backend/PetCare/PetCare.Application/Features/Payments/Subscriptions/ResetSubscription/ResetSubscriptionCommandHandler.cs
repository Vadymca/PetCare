namespace PetCare.Application.Features.Payments.Subscriptions.ResetSubscription;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;
using PetCare.Domain.Enums;

/// <summary>
/// Handles the process of resetting a recurring subscription, including
/// canceling the old subscription and creating a new LiqPay recurring contract.
/// </summary>
public sealed class ResetSubscriptionCommandHandler
    : IRequestHandler<ResetSubscriptionCommand, SubscriptionDto>
{
    private readonly ILiqPayService liqPayService;
    private readonly IPaymentService paymentService;
    private readonly ILogger<ResetSubscriptionCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResetSubscriptionCommandHandler"/> class with the specified payment and logging.
    /// services.
    /// </summary>
    /// <param name="liqPayService">The service used to interact with the LiqPay payment gateway. Cannot be null.</param>
    /// <param name="paymentService">The service responsible for handling payment operations. Cannot be null.</param>
    /// <param name="logger">The logger used to record diagnostic and operational information. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if liqPayService, paymentService, or logger is null.</exception>
    public ResetSubscriptionCommandHandler(
        ILiqPayService liqPayService,
        IPaymentService paymentService,
        ILogger<ResetSubscriptionCommandHandler> logger)
    {
        this.liqPayService = liqPayService ?? throw new ArgumentNullException(nameof(liqPayService));
        this.paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Resets a user's subscription by creating a new recurring contract and updating the subscription details.
    /// </summary>
    /// <param name="command">The command containing information required to reset the subscription, including the user ID, old subscription
    /// ID, amount, currency, scope, and scope ID.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SubscriptionDto with the updated
    /// subscription details.</returns>
    public async Task<SubscriptionDto> Handle(
        ResetSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        this.logger.LogInformation(
            "Resetting subscription {OldId} for user {UserId}",
            command.OldSubscriptionId,
            command.UserId);

        // 1. string → enum
        var scope = command.Scope switch
        {
            "Guardianship" => SubscriptionScope.Guardianship,
            "AidRequest" => SubscriptionScope.AidRequest,
            _ => SubscriptionScope.Global,
        };

        // 2. Створюємо новий LiqPay контракт
        var liqResp = await this.liqPayService.CreateRecurringContractAsync(
            command.UserId,
            command.Amount,
            command.Currency,
            scope,
            command.ScopeId,
            cancellationToken);

        // 3. Оновлюємо локальні підписки через PaymentService
        var newSub = await this.paymentService.ResetSubscriptionAsync(
            command.OldSubscriptionId,
            command.UserId,
            command.Amount,
            command.Currency,
            scope,
            command.ScopeId,
            provider: "LiqPay",
            paymentMethodId: liqResp.PaymentMethodId,
            providerSubscriptionId: liqResp.ProviderSubscriptionId,
            nextChargeAt: liqResp.NextChargeAt,
            cancellationToken: cancellationToken);

        return new SubscriptionDto(
            newSub.Id,
            newSub.UserId,
            newSub.Amount,
            newSub.Currency,
            newSub.Provider,
            newSub.ProviderSubscriptionId,
            newSub.NextChargeAt,
            newSub.Status.ToString(),
            newSub.ScopeType.ToString(),
            newSub.ScopeId);
    }
}
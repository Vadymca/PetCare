namespace PetCare.Application.Features.Payments.Subscriptions.ResetSubscription;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;
using PetCare.Domain.Enums;

/// <summary>
/// Handles the process of resetting a recurring subscription by cancelling the old subscription,
/// creating a new local subscription, and generating a LiqPay checkout for the new subscription.
/// </summary>
public sealed class ResetSubscriptionCommandHandler
    : IRequestHandler<ResetSubscriptionCommand, LiqPayCheckoutResponseDto>
{
    private readonly IPaymentService paymentService;
    private readonly ILiqPayClient liqPayClient;
    private readonly IGuardianshipService guardianshipService;
    private readonly IAnimalService animalService;
    private readonly IPaymentIntentService paymentIntentService;
    private readonly ILogger<ResetSubscriptionCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResetSubscriptionCommandHandler"/> class.
    /// </summary>
    /// <param name="paymentService">The payment service.</param>
    /// <param name="liqPayClient">The LiqPay client.</param>
    /// <param name="guardianshipService">The guardianship service.</param>
    /// <param name="animalService">The animal service.</param>
    /// <param name="paymentIntentService">The payment intent service.</param>
    /// <param name="logger">The logger.</param>
    public ResetSubscriptionCommandHandler(
        IPaymentService paymentService,
        ILiqPayClient liqPayClient,
        IGuardianshipService guardianshipService,
        IAnimalService animalService,
        IPaymentIntentService paymentIntentService,
        ILogger<ResetSubscriptionCommandHandler> logger)
    {
        this.paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        this.liqPayClient = liqPayClient ?? throw new ArgumentNullException(nameof(liqPayClient));
        this.guardianshipService = guardianshipService ?? throw new ArgumentNullException(nameof(guardianshipService));
        this.animalService = animalService ?? throw new ArgumentNullException(nameof(animalService));
        this.paymentIntentService = paymentIntentService ?? throw new ArgumentNullException(nameof(paymentIntentService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<LiqPayCheckoutResponseDto> Handle(
        ResetSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Отримуємо стару підписку
        var oldSub = await this.paymentService
            .FindSubscriptionByIdOrProviderIdAsync(command.SubscriptionId, cancellationToken)
            ?? throw new KeyNotFoundException("Підписку не знайдено.");

        // 2. Визначаємо суму і валюту
        decimal amount;
        string currency;
        if (oldSub.ScopeType == SubscriptionScope.Guardianship)
        {
            var guardianship = oldSub.ScopeId.HasValue
                ? await this.guardianshipService.GetByIdAsync(oldSub.ScopeId.Value, cancellationToken)
                : null;

            var animal = guardianship?.AnimalId != null
                ? await this.animalService.GetByIdAsync(guardianship.AnimalId, cancellationToken)
                : null;

            if (animal == null)
            {
                throw new InvalidOperationException("Тварину не знайдено.");
            }

            amount = (decimal)animal.CareCost;
            currency = "UAH";
        }
        else
        {
            amount = oldSub.Amount;
            currency = oldSub.Currency;
        }

        // 3. Скидаємо стару підписку та створюємо нову локальну
        var newSub = await this.paymentService.ResetSubscriptionAsync(
            oldSubscriptionId: oldSub.Id,
            userId: oldSub.UserId,
            amount: amount,
            currency: currency,
            scope: oldSub.ScopeType,
            scopeId: oldSub.ScopeId,
            provider: oldSub.Provider,
            paymentMethodId: oldSub.PaymentMethodId,
            providerSubscriptionId: Guid.NewGuid().ToString(), // новий providerSubscriptionId
            nextChargeAt: null,
            cancellationToken);

        this.logger.LogInformation(
            "Old subscription {OldId} reset and new subscription {NewId} created for user {UserId}.",
            oldSub.Id,
            newSub.Id,
            oldSub.UserId);

        // 4. Створюємо LiqPay intent для нової підписки
        var intent = await this.paymentIntentService.CreateLiqPayIntentAsync(
            newSub.ScopeType,
            newSub.ScopeId,
            newSub.UserId,
            amount,
            currency,
            isRecurring: true,
            anonymous: false,
            cancellationToken);

        // 5. Формуємо опис, null-безпечний
        string userName = oldSub.User != null
            ? $"{oldSub.User.FirstName} {oldSub.User.LastName}"
            : "анонім";

        string description;
        if (newSub.ScopeType == SubscriptionScope.Guardianship && newSub.ScopeId.HasValue)
        {
            var guardianship = await this.guardianshipService.GetByIdAsync(newSub.ScopeId.Value, cancellationToken);
            var animal = guardianship?.AnimalId != null
                ? await this.animalService.GetByIdAsync(guardianship.AnimalId, cancellationToken)
                : null;

            description = animal != null
                ? $"Ви відновлюєте підписку на опіку для {animal.Name} (опікун: {userName})"
                : $"Ви відновлюєте підписку на опіку (опікун: {userName})";
        }
        else
        {
            description = $"Відновлення підписки для користувача: {userName}";
        }

        // 6. Формуємо DTO для checkout
        var dto = new CreateLiqPayCheckoutDto(
            Amount: amount,
            Currency: currency,
            Description: description,
            IsRecurring: true,
            Scope: newSub.ScopeType,
            EntityId: newSub.ScopeId,
            UserId: newSub.UserId,
            Anonymous: false,
            PayerName: oldSub.User?.FirstName ?? string.Empty,
            PayerPhone: oldSub.User?.Phone,
            PayerEmail: oldSub.User?.Email);

        // 7. Генеруємо LiqPay checkout
        var checkout = await this.liqPayClient.BuildCheckoutAsync(
            dto,
            intent.ExternalOrderId,
            cancellationToken);

        this.logger.LogInformation(
            "LiqPay checkout generated for new subscription {NewId}.",
            newSub.Id);

        return checkout;
    }
}
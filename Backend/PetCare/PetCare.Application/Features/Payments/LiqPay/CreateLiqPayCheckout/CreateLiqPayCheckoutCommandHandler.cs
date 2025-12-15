namespace PetCare.Application.Features.Payments.LiqPay.CreateLiqPayCheckout;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;
using PetCare.Domain.Enums;

/// <summary>
/// Handles requests to create a LiqPay checkout by processing the command and generating a checkout response.
/// </summary>
/// <remarks>This handler uses an injected ILiqPayClient to build the checkout asynchronously. It validates that
/// the requested amount is greater than zero before proceeding. Typically used within a MediatR pipeline to facilitate
/// payment operations via LiqPay.</remarks>
public sealed class CreateLiqPayCheckoutCommandHandler
 : IRequestHandler<CreateLiqPayCheckoutCommand, LiqPayCheckoutResponseDto>
{
    private readonly ILiqPayClient liqPayClient;
    private readonly IGuardianshipService guardianshipService;
    private readonly IAnimalService animalService;
    private readonly IPaymentIntentService paymentIntentService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLiqPayCheckoutCommandHandler"/> class using the specified LiqPay client.
    /// </summary>
    /// <param name="liqPayClient">The client used to interact with the LiqPay payment service. Cannot be null.</param>
    /// <param name="guardianshipService">The service used to manage guardianships. Cannot be null.</param>
    /// <param name="animalService">The service used to manage animals. Cannot be null.</param>
    /// <param name="paymentIntentService">The service used to manage payment intents. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="liqPayClient"/> is null.</exception>
    public CreateLiqPayCheckoutCommandHandler(
        ILiqPayClient liqPayClient,
        IGuardianshipService guardianshipService,
        IAnimalService animalService,
        IPaymentIntentService paymentIntentService)
    {
        this.liqPayClient = liqPayClient ?? throw new ArgumentNullException(nameof(liqPayClient));
        this.guardianshipService = guardianshipService ?? throw new ArgumentNullException(nameof(guardianshipService));
        this.animalService = animalService ?? throw new ArgumentNullException(nameof(animalService));
        this.paymentIntentService = paymentIntentService ?? throw new ArgumentNullException(nameof(paymentIntentService));
    }

    /// <inheritdoc/>
    public async Task<LiqPayCheckoutResponseDto> Handle(CreateLiqPayCheckoutCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;

        decimal amount = req.Amount;
        string currency = req.Currency ?? "UAH";
        bool isRecurring = req.IsRecurring;
        string? description = req.Description;

        Guid? userId = req.UserId ?? command.TokenUserId;
        string? payerName = req.PayerName ?? command.TokenPayerName;
        string? payerPhone = req.PayerPhone ?? command.TokenPayerPhone;
        string? payerEmail = req.PayerEmail ?? command.TokenPayerEmail;

        if (req.Scope == SubscriptionScope.Guardianship)
        {
            if (req.EntityId is null)
            {
                throw new InvalidOperationException("Для опіки потрібно передати EntityId.");
            }

            // 1. Отримуємо опіку через сервіс
            var guardianship = await this.guardianshipService
                .GetByIdAsync(req.EntityId.Value, cancellationToken)
                ?? throw new InvalidOperationException("Опіку не знайдено.");

            // 2. Отримуємо тварину через сервіс
            var animal = await this.animalService
                .GetByIdAsync(guardianship.AnimalId, cancellationToken)
                ?? throw new InvalidOperationException("Тварину не знайдено.");

            // 3. Amount завжди з careCost тварини
            amount = (decimal)animal.CareCost;

            // 4. Опіка завжди рекурентна
            isRecurring = true;

            // 5. Валюта завжди UAH
            currency = "UAH";

            // 6. Дані платника, якщо не передані
            payerName ??= command.TokenPayerName;
            payerPhone ??= command.TokenPayerPhone;
            payerEmail ??= command.TokenPayerEmail;

            // 7. UserId, якщо не переданий
            userId ??= command.TokenUserId;
        }
        else
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Сума має бути більшою за 0.");
            }

            payerName ??= command.TokenPayerName;
            payerPhone ??= command.TokenPayerPhone;
            payerEmail ??= command.TokenPayerEmail;
            userId ??= command.TokenUserId;
        }

        bool isAnonymous =
            userId is null &&
            string.IsNullOrWhiteSpace(payerName);

        var intent = await this.paymentIntentService.CreateLiqPayIntentAsync(
            req.Scope,
            req.EntityId,
            userId,
            amount,
            currency,
            isRecurring,
            anonymous: isAnonymous,
            payerName,
            cancellationToken);

        var finalDto = new CreateLiqPayCheckoutDto(
            Amount: amount,
            Currency: currency,
            Description: description,
            IsRecurring: isRecurring,
            Scope: req.Scope,
            EntityId: req.EntityId,
            UserId: userId,
            Anonymous: isAnonymous,
            PayerName: payerName,
            PayerPhone: payerPhone,
            PayerEmail: payerEmail);

        var checkout = await this.liqPayClient.BuildCheckoutAsync(
            finalDto,
            intent.ExternalOrderId,
            cancellationToken);

        return checkout;
    }
}

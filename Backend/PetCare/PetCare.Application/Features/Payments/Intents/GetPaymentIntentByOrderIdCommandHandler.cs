namespace PetCare.Application.Features.Payments.Intents;

using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.Payments;
using PetCare.Application.Interfaces;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;

/// <summary>
/// Handles the retrieval of payment intent details by external order ID.
/// </summary>
/// <remarks>This handler processes a GetPaymentIntentByOrderIdCommand and returns detailed information about the
/// associated payment intent, including related donation, guardianship, or subscription details if available. Typically
/// used in scenarios where payment intent information must be fetched for a specific order in the system.</remarks>
public sealed class GetPaymentIntentByOrderIdCommandHandler
 : IRequestHandler<GetPaymentIntentByOrderIdCommand, PaymentIntentDetailsDto?>
{
    private readonly IPaymentIntentService paymentIntentService;
    private readonly IAnimalService animalService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPaymentIntentByOrderIdCommandHandler"/> class with the specified payment.
    /// intent and animal services.
    /// </summary>
    /// <param name="paymentIntentService">The service used to manage and retrieve payment intents.</param>
    /// <param name="animalService">The service used to access animal-related data required for payment intent operations.</param>
    public GetPaymentIntentByOrderIdCommandHandler(
        IPaymentIntentService paymentIntentService,
        IAnimalService animalService)
    {
        this.paymentIntentService = paymentIntentService;
        this.animalService = animalService;
    }

    /// <summary>
    /// Retrieves the details of a payment intent associated with the specified order identifier.
    /// </summary>
    /// <param name="command">The command containing the external order identifier for which to retrieve the payment intent details. Cannot be
    /// null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="PaymentIntentDetailsDto"/> containing the details of the payment intent if found; otherwise, <see
    /// langword="null"/>.</returns>
    public async Task<PaymentIntentDetailsDto?> Handle(
        GetPaymentIntentByOrderIdCommand command,
        CancellationToken cancellationToken)
    {
        var intent = await this.paymentIntentService
            .GetByExternalOrderIdAsync(command.ExternalOrderId, cancellationToken);

        if (intent is null)
        {
            return null;
        }

        GuardianshipDetailsDto? guardianshipDto = null;
        if (intent.Guardianship is not null)
        {
            var animal = await this.animalService
                .GetByIdAsync(intent.Guardianship.AnimalId, cancellationToken);

            guardianshipDto = new GuardianshipDetailsDto(
                Id: intent.Guardianship.Id,
                AnimalId: intent.Guardianship.AnimalId,
                AnimalName: animal?.Name.Value ?? "Тварину не знайдено",
                Status: intent.Guardianship.Status.ToString(),
                StartDate: intent.Guardianship.StartDate,
                GraceUntil: intent.Guardianship.GraceUntil);
        }

        DonationDetailsDto? donationDto = null;
        if (intent.Donation is not null)
        {
            donationDto = new DonationDetailsDto(
                Id: intent.Donation.Id,
                Amount: intent.Donation.Amount,
                Currency: intent.Donation.Currency,
                Purpose: intent.Donation.Purpose,
                Status: intent.Donation.Status.ToString(),
                TransactionId: intent.Donation.TransactionId,
                TargetEntity: intent.Donation.TargetEntity,
                TargetEntityId: intent.Donation.TargetEntityId);
        }

        SubscriptionDetailsDto? subscriptionDto = null;
        if (intent.Subscription is not null)
        {
            subscriptionDto = new SubscriptionDetailsDto(
                Id: intent.Subscription.Id,
                Amount: intent.Subscription.Amount,
                Currency: intent.Subscription.Currency,
                Provider: intent.Subscription.Provider,
                ProviderSubscriptionId: intent.Subscription.ProviderSubscriptionId,
                Status: intent.Subscription.Status.ToString(),
                LastChargeAt: intent.Subscription.LastChargeAt,
                NextChargeAt: intent.Subscription.NextChargeAt);
        }

        string? message = BuildUxMessage(
            intent,
            guardianshipDto,
            donationDto,
            subscriptionDto);

        return new PaymentIntentDetailsDto(
            OrderId: intent.ExternalOrderId,
            Status: intent.Status,
            ProviderPaymentId: intent.ProviderPaymentId,
            Scope: intent.ScopeType,
            ScopeId: intent.ScopeId,
            UserId: intent.UserId,
            Amount: intent.Amount,
            Currency: intent.Currency,
            IsRecurring: intent.IsRecurring,
            Anonymous: intent.Anonymous,
            Donation: donationDto,
            Guardianship: guardianshipDto,
            Subscription: subscriptionDto,
            CreatedAt: intent.CreatedAt,
            UpdatedAt: intent.UpdatedAt,
            Message: message);
    }

    private static string? BuildUxMessage(
        PaymentIntent intent,
        GuardianshipDetailsDto? guardianship,
        DonationDetailsDto? donation,
        SubscriptionDetailsDto? subscription)
    {
        if (intent.ScopeType == SubscriptionScope.Guardianship &&
            guardianship is not null)
        {
            return intent.IsRecurring
                ? $"Ви успішно оформили щомісячну опіку над {guardianship.AnimalName}."
                : $"Ви успішно оплатили опіку над {guardianship.AnimalName}.";
        }

        if (donation is not null)
        {
            if (donation.TargetEntity == "AidRequest")
            {
                return $"Дякуємо! Ви підтримали запит допомоги на суму {donation.Amount} {donation.Currency}.";
            }

            if (donation.TargetEntity == "Global")
            {
                return $"Дякуємо за ваш внесок {donation.Amount} {donation.Currency}.";
            }

            return $"Платіж успішно виконано на суму {donation.Amount} {donation.Currency}.";
        }

        return null;
    }
}

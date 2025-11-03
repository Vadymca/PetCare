namespace PetCare.Infrastructure.Services;

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;

/// <summary>
/// Provides operations for creating and managing payment subscriptions associated with guardianships using the LiqPay
/// payment provider.
/// </summary>
/// <remarks>This service encapsulates the logic required to create payment subscriptions scoped to specific
/// guardianships and persists them to the database. It relies on an application database context for data access and
/// enforces validation on subscription creation parameters. All payment subscriptions created through this service use
/// the LiqPay provider. Thread safety and transaction management are determined by the underlying database context
/// implementation.</remarks>
public class SubscriptionService : ISubscriptionService
{
    private readonly IGuardianshipRepository guardianships;
    private readonly ILogger<SubscriptionService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionService"/> class.
    /// </summary>
    /// <param name="guardianships">The repository used for data access via the Guardianship aggregate.</param>
    /// <param name="logger">The logger instance for logging operations within the service.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="guardianships"/> is null.</exception>
    public SubscriptionService(
        IGuardianshipRepository guardianships,
        ILogger<SubscriptionService> logger)
    {
        this.guardianships = guardianships ?? throw new ArgumentNullException(nameof(guardianships));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new payment subscription for a guardianship using the specified user, guardianship, amount, and
    /// currency.
    /// </summary>
    /// <remarks>The payment subscription is created using the LiqPay provider and is scoped to the
    /// specified guardianship. The subscription is persisted to the database before being returned.</remarks>
    /// <param name="userId">The unique identifier of the user for whom the payment subscription is being created.</param>
    /// <param name="guardianshipId">The unique identifier of the guardianship associated with the payment subscription.</param>
    /// <param name="amount">The payment amount for the subscription. Must be greater than zero.</param>
    /// <param name="currency">The ISO currency code representing the currency of the payment (for example, "USD" or "UAH").</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A PaymentSubscription instance representing the newly created payment subscription for the guardianship.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the specified amount is less than or equal to zero.</exception>
    public async Task<PaymentSubscription> CreateForGuardianshipAsync(Guid userId, Guid guardianshipId, decimal amount, string currency, CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Сума має бути більшою за 0.");
        }

        var provider = "LiqPay";
        var methodId = await this.guardianships.RequirePaymentMethodIdByProviderAsync(provider, cancellationToken);

        var subscription = PaymentSubscription.Create(
            userId,
            methodId,
            SubscriptionScope.Guardianship,
            guardianshipId,
            amount,
            currency,
            provider,
            Guid.NewGuid().ToString("N"));

        await this.guardianships.AddSubscriptionAsync(subscription, cancellationToken);
        return subscription;
    }

    /// <summary>
    /// Cancels all active payment subscriptions associated with the specified guardianship.
    /// </summary>
    /// <remarks>All active subscriptions scoped to the specified guardianship will be cancelled. This
    /// operation is performed asynchronously and will persist changes to the database upon completion.</remarks>
    /// <param name="guardianshipId">The unique identifier of the guardianship for which active payment subscriptions will be cancelled.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous cancellation operation.</returns>
    public async Task CancelForGuardianshipAsync(Guid guardianshipId, CancellationToken cancellationToken = default)
    {
        await this.guardianships.CancelSubscriptionsByScopeAsync(
            SubscriptionScope.Guardianship,
            guardianshipId,
            cancellationToken);
    }

    /// <summary>
    /// Creates a new global payment subscription for the specified user with the given amount and currency.
    /// </summary>
    /// <remarks>The subscription is created with a global scope and uses the LiqPay payment provider. The
    /// method persists the subscription to the database before returning it.</remarks>
    /// <param name="userId">The unique identifier of the user for whom the subscription is created. If null, a default value is used.</param>
    /// <param name="amount">The monetary amount for the subscription. Must be a positive value.</param>
    /// <param name="currency">The ISO currency code representing the currency for the subscription. For example, "USD" or "EUR".</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="PaymentSubscription"/> instance representing the newly created global payment subscription.</returns>
    public async Task<PaymentSubscription> CreateGlobalAsync(
        Guid? userId,
        decimal amount,
        string currency,
        CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Сума має бути більшою за 0.");
        }

        var provider = "LiqPay";
        var methodId = await this.guardianships.RequirePaymentMethodIdByProviderAsync(provider, cancellationToken);

        var subscription = PaymentSubscription.Create(
            userId ?? Guid.Empty,
            methodId,
            SubscriptionScope.Global,
            null,
            amount,
            currency,
            provider,
            Guid.NewGuid().ToString("N"));

        await this.guardianships.AddSubscriptionAsync(subscription, cancellationToken);
        return subscription;
    }

    /// <summary>
    /// Creates a new payment subscription for the specified aid request and user.
    /// </summary>
    /// <remarks>The payment subscription is created using the LiqPay provider and is persisted to the
    /// database upon successful completion of the operation.</remarks>
    /// <param name="userId">The unique identifier of the user associated with the subscription. If null, a default value is used.</param>
    /// <param name="aidRequestId">The unique identifier of the aid request for which the payment subscription is created.</param>
    /// <param name="amount">The payment amount to be associated with the subscription. Must be a positive value.</param>
    /// <param name="currency">The ISO currency code representing the currency of the payment (e.g., "USD", "EUR").</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A PaymentSubscription instance representing the newly created payment subscription for the aid request.</returns>
    public async Task<PaymentSubscription> CreateForAidRequestAsync(
        Guid? userId,
        Guid aidRequestId,
        decimal amount,
        string currency,
        CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Сума має бути більшою за 0.");
        }

        var provider = "LiqPay";
        var methodId = await this.guardianships.RequirePaymentMethodIdByProviderAsync(provider, cancellationToken);

        var subscription = PaymentSubscription.Create(
            userId ?? Guid.Empty,
            methodId,
            SubscriptionScope.AidRequest,
            aidRequestId,
            amount,
            currency,
            provider,
            Guid.NewGuid().ToString("N"));

        await this.guardianships.AddSubscriptionAsync(subscription, cancellationToken);
        return subscription;
    }

    /// <summary>
    /// Asynchronously cancels an active payment subscription identified by the specified provider subscription ID.
    /// </summary>
    /// <param name="providerSubscriptionId">The unique identifier of the payment subscription to cancel. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous cancel operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a subscription with the specified provider subscription ID does not exist.</exception>
    public async Task CancelAsync(string providerSubscriptionId, CancellationToken cancellationToken = default)
    {
        await this.guardianships.CancelSubscriptionByProviderIdAsync(providerSubscriptionId, cancellationToken);
    }

    /// <summary>
    /// Retrieves a read-only list of recurring payment subscriptions associated with the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose recurring payment subscriptions are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of <see cref="PaymentSubscription"/> objects for the specified user, ordered by creation date
    /// in descending order. Returns an empty list if no subscriptions are found.</returns>
    public async Task<IReadOnlyList<PaymentSubscription>> GetMyRecurringAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await this.guardianships.GetUserSubscriptionsAsync(userId, cancellationToken);
    }

    /// <summary>
    /// Retrieves the list of active payment subscriptions for the specified user, along with the date of the next
    /// expected charge for each subscription.
    /// </summary>
    /// <remarks>Only subscriptions with an active status are included in the result. The method does not
    /// track changes to the returned entities.</remarks>
    /// <param name="userId">The unique identifier of the user whose active payment subscriptions are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of tuples, each containing a payment subscription and the date of its next expected charge. The
    /// date is <see langword="null"/> if no upcoming charge is scheduled.</returns>
    public async Task<IReadOnlyList<(PaymentSubscription Subscription, DateTime? NextChargeAt)>> GetMyExpectedPaymentsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await this.guardianships.GetUserExpectedChargesAsync(userId, cancellationToken);
    }

    /// <summary>
    /// Asynchronously retrieves a payment subscription associated with the specified provider subscription identifier.
    /// </summary>
    /// <param name="providerSubscriptionId">The unique identifier assigned to the subscription by the external payment provider. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="PaymentSubscription"/> object if a matching subscription is found; otherwise, <see
    /// langword="null"/>.</returns>
    public async Task<PaymentSubscription?> FindByProviderSubscriptionIdAsync(string providerSubscriptionId, CancellationToken cancellationToken = default)
    {
        return await this.guardianships.FindByProviderSubscriptionIdAsync(providerSubscriptionId, cancellationToken);
    }

    /// <summary>
    /// Cancels all active user subscriptions that have expired based on their next scheduled charge date.
    /// </summary>
    /// <remarks>A subscription is considered expired if it is active and its next scheduled charge date is
    /// more than three days in the past. The method logs information about each canceled subscription and any errors
    /// encountered during cancellation. If no expired subscriptions are found, the method returns zero.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>The number of subscriptions that were canceled.</returns>
    public async Task<int> CancelExpiredAsync(CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Checking for expired subscriptions...");

        var utcNow = DateTime.UtcNow;

        // Отримуємо всі підписки користувачів
        var allSubs = await this.guardianships.ListAllSubscriptionsAsync(cancellationToken);

        // Фільтруємо лише активні і прострочені
        var expiredSubs = allSubs
            .Where(s =>
                s.Status == SubscriptionStatus.Active &&
                s.NextChargeAt.HasValue &&
                s.NextChargeAt.Value.AddDays(3) < utcNow)
            .ToList();

        if (expiredSubs.Count == 0)
        {
            this.logger.LogDebug("No expired subscriptions found.");
            return 0;
        }

        // Відміняємо через репозиторій
        foreach (var sub in expiredSubs)
        {
            try
            {
                await this.guardianships.CancelSubscriptionByProviderIdAsync(
                    sub.ProviderSubscriptionId!,
                    cancellationToken);

                this.logger.LogInformation(
                    "Canceled expired subscription {SubId} ({ProviderId}) for user {UserId}.",
                    sub.Id,
                    sub.ProviderSubscriptionId,
                    sub.UserId);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    ex,
                    "Error canceling subscription {ProviderId} for user {UserId}.",
                    sub.ProviderSubscriptionId,
                    sub.UserId);
            }
        }

        this.logger.LogInformation("Canceled {Count} expired subscriptions.", expiredSubs.Count);
        return expiredSubs.Count;
    }
}

namespace PetCare.Infrastructure.Services;

using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;

/// <summary>
/// Provides functionality for processing payments through the application's payment system.
/// </summary>
/// <remarks>Implementations of this service typically handle payment transactions, validation, and integration
/// with external payment providers. Consumers should use this service to initiate and manage payment operations rather
/// than interacting directly with payment gateways.</remarks>
public class PaymentService : IPaymentService
{
    private readonly IGuardianshipRepository guardianships;
    private readonly IGuardianshipService guardianshipService;
    private readonly ILogger<PaymentService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentService"/> class with the specified database context and guardianship.
    /// repository.
    /// </summary>
    /// <param name="db">The database context used for accessing and managing payment-related data.</param>
    /// <param name="guardianships">The repository used to retrieve and manage guardianship information associated with payments.</param>
    /// <param name="guardianshipService">The service for managing guardianship-related operations.</param>
    /// <param name="logger">The logger instance for logging payment service operations and events.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="db"/> or <paramref name="guardianships"/> is null.</exception>
    public PaymentService(
        IGuardianshipRepository guardianships,
        IGuardianshipService guardianshipService,
        ILogger<PaymentService> logger)
    {
        this.guardianships = guardianships ?? throw new ArgumentNullException(nameof(guardianships));
        this.guardianshipService = guardianshipService ?? throw new ArgumentNullException(nameof(guardianshipService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Records a successful payment transaction as a completed donation and updates related entities as needed.
    /// </summary>
    /// <remarks>If the donation targets a guardianship and is the first payment, the guardianship will be
    /// activated and linked to the donation. The method also persists the donation to the database and may update
    /// related entities based on business rules.</remarks>
    /// <param name="provider">The name of the payment provider used to process the transaction. Must correspond to a supported provider.</param>
    /// <param name="transactionId">The unique identifier of the transaction as provided by the payment provider.</param>
    /// <param name="amount">The amount of the donation. Must be greater than zero.</param>
    /// <param name="currency">The currency code for the donation amount (for example, "USD" or "EUR").</param>
    /// <param name="targetEntity">The type of entity that the donation is intended to support (for example, "Guardianship").</param>
    /// <param name="targetEntityId">The unique identifier of the target entity to associate with the donation, or null if not applicable.</param>
    /// <param name="recurring">Indicates whether the donation is part of a recurring payment schedule. Set to <see langword="true"/> for
    /// recurring donations; otherwise, <see langword="false"/>.</param>
    /// <param name="anonymous">Indicates whether the donation should be recorded as anonymous. Set to <see langword="true"/> to hide donor
    /// identity; otherwise, <see langword="false"/>.</param>
    /// <param name="userId">The unique identifier of the user making the donation, or null if the donor is not registered.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
    /// <returns>A <see cref="Donation"/> object representing the recorded donation, including all associated metadata and links
    /// to related entities.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <paramref name="amount"/> is less than or equal to zero.</exception>
    public async Task<Donation> RecordChargeSuccessAsync(
        string provider,
        string transactionId,
        decimal amount,
        string currency,
        string targetEntity,
        Guid? targetEntityId,
        bool recurring,
        bool anonymous,
        Guid? userId,
        CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Сума має бути більшою за 0.");
        }

        var paymentMethodId = await this.guardianships.RequirePaymentMethodIdByProviderAsync(provider, cancellationToken);

        var donation = Donation.Create(
            userId,
            amount,
            currency,
            shelterId: null,
            paymentMethodId,
            DonationStatus.Completed,
            transactionId,
            purpose: BuildPurpose(targetEntity),
            recurring,
            anonymous,
            DateTime.UtcNow,
            report: null);

        donation.SetTarget(targetEntity, targetEntityId);

        await this.guardianships.AddDonationAsync(donation, cancellationToken);

        // Якщо це опіка — активуємо Guardianship
        if (targetEntity == "Guardianship" && targetEntityId is not null)
        {
            try
            {
                await this.guardianships.LinkDonationAsync(targetEntityId.Value, donation.Id, cancellationToken);
                await this.guardianshipService.ActivateWithFirstPaymentAsync(targetEntityId.Value, donation.Id, cancellationToken);

                this.logger.LogInformation(
                    "Guardianship {GuardianshipId} activated successfully after first payment {DonationId}.",
                    targetEntityId,
                    donation.Id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    ex,
                    "Error while activating guardianship {GuardianshipId} after first payment {DonationId}.",
                    targetEntityId,
                    donation.Id);
                throw;
            }
        }

        // Якщо recurring — створюємо PaymentSubscription
        if (recurring && userId.HasValue)
        {
            try
            {
                var scope = targetEntity switch
                {
                    "Guardianship" => SubscriptionScope.Guardianship,
                    "AnimalAidRequest" => SubscriptionScope.AidRequest,
                    _ => SubscriptionScope.Global,
                };

                var subscription = PaymentSubscription.Create(
                    userId.Value,
                    paymentMethodId,
                    scope,
                    targetEntityId,
                    amount,
                    currency,
                    provider,
                    providerSubscriptionId: transactionId); // тимчасово, доки не інтегруємо LiqPay subscription_id

                await this.guardianships.AddSubscriptionAsync(subscription, cancellationToken);

                this.logger.LogInformation(
                    "Created new PaymentSubscription {SubId} for user {UserId} via {Provider}.",
                    subscription.Id,
                    userId,
                    provider);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to create PaymentSubscription for recurring payment.");
            }
        }

        this.logger.LogInformation(
            "Payment {TxId} recorded successfully as donation {DonationId}.",
            transactionId,
            donation.Id);

        return donation;
    }

    /// <summary>
    /// Records a failed donation attempt for the specified payment provider and target entity.
    /// </summary>
    /// <remarks>If the target entity is a guardianship and the guardianship is not completed, a payment
    /// requirement will be set for the entity. The method adds the failed donation to the database and may update
    /// related entities as needed.</remarks>
    /// <param name="provider">The name of the payment provider associated with the failed charge. Cannot be null or empty.</param>
    /// <param name="transactionId">The transaction identifier provided by the payment provider, if available. May be null if not applicable.</param>
    /// <param name="amount">The monetary amount of the attempted donation. Must be a non-negative value.</param>
    /// <param name="currency">The ISO currency code representing the currency of the donation. Cannot be null or empty.</param>
    /// <param name="targetEntity">The type of entity that the donation was intended for, such as a project or guardianship. Cannot be null or
    /// empty.</param>
    /// <param name="targetEntityId">The unique identifier of the target entity, if applicable. May be null if the donation is not associated with a
    /// specific entity.</param>
    /// <param name="recurring">Indicates whether the donation was intended to be a recurring payment. Set to <see langword="true"/> for
    /// recurring donations; otherwise, <see langword="false"/>.</param>
    /// <param name="anonymous">Indicates whether the donor chose to remain anonymous. Set to <see langword="true"/> if the donation is
    /// anonymous; otherwise, <see langword="false"/>.</param>
    /// <param name="userId">The unique identifier of the user who attempted the donation, if available. May be null for anonymous donations.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Donation"/> object representing the recorded failed donation attempt.</returns>
    public async Task<Donation> RecordChargeFailedAsync(
        string provider,
        string? transactionId,
        decimal amount,
        string currency,
        string targetEntity,
        Guid? targetEntityId,
        bool recurring,
        bool anonymous,
        Guid? userId,
        CancellationToken cancellationToken = default)
    {
        var paymentMethodId = await this.guardianships.RequirePaymentMethodIdByProviderAsync(provider, cancellationToken);

        var donation = Donation.Create(
            userId,
            amount,
            currency,
            shelterId: null,
            paymentMethodId,
            DonationStatus.Failed,
            transactionId,
            purpose: BuildPurpose(targetEntity),
            recurring,
            anonymous,
            DateTime.UtcNow,
            report: null);

        donation.SetTarget(targetEntity, targetEntityId);

        await this.guardianships.AddDonationAsync(donation, cancellationToken);

        // Якщо це опіка — ставимо RequiresPayment
        if (targetEntity == "Guardianship" && targetEntityId is not null)
        {
            try
            {
                await this.guardianshipService.RequirePaymentAsync(targetEntityId.Value, 3, cancellationToken);
                this.logger.LogWarning(
                    "Guardianship {GuardianshipId} reverted to RequiresPayment due to failed payment {DonationId}.",
                    targetEntityId,
                    donation.Id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while reverting guardianship {GuardianshipId} after failed payment.", targetEntityId);
            }
        }

        return donation;
    }

    /// <summary>
    /// Builds a payment status URL by appending the specified status and additional query parameters to the provided
    /// base path.
    /// </summary>
    /// <param name="basePath">The base URL to which the payment status and query parameters will be appended.</param>
    /// <param name="status">The payment status to include in the query string. Typical values are "success", "failed", or "pending".</param>
    /// <param name="data">A collection of key-value pairs representing additional query parameters to include in the URL. Keys with null
    /// or whitespace values are omitted.</param>
    /// <returns>A string containing the constructed URL with the payment status and any additional query parameters appended as
    /// a query string.</returns>
    public string BuildPaymentStatusUrl(string basePath, string status, IDictionary<string, string?> data)
    {
        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        query["status"] = status; // success/failed/pending
        foreach (var kv in data)
        {
            if (!string.IsNullOrWhiteSpace(kv.Value))
            {
                query[kv.Key] = kv.Value;
            }
        }

        return $"{basePath}?{query}";
    }

    /// <summary>
    /// Asynchronously retrieves a read-only list of donations made by the specified user, ordered by most recent
    /// donation date.
    /// </summary>
    /// <remarks>The returned list is ordered in descending order by donation date, with the most recent
    /// donations appearing first. The query is performed without tracking changes to the retrieved entities.</remarks>
    /// <param name="userId">The unique identifier of the user whose donation payments are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of <see cref="Donation"/> objects representing the user's donation payments. The list is empty
    /// if the user has not made any donations.</returns>
    public async Task<IReadOnlyList<Donation>> GetMyPaymentsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
         return await this.guardianships.GetUserDonationsAsync(userId, cancellationToken);
    }

    /// <summary>
    /// Retrieves all donations ordered by most recent first.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of <see cref="Donation"/> objects representing all recorded donations.</returns>
    public async Task<IReadOnlyList<Donation>> ListAllDonationsAsync(CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Retrieving all donations ordered by most recent first.");

        var donations = await this.guardianships.ListAllDonationsAsync(cancellationToken);

        this.logger.LogInformation("Retrieved {Count} total donations.", donations.Count);

        return donations;
    }

    /// <summary>
    /// Retrieves donations filtered by a specific project (e.g., AidRequest, Shelter).
    /// </summary>
    /// <param name="projectId">The unique identifier of the project for which to retrieve donations.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of <see cref="Donation"/> objects representing donations related to the specified project.</returns>
    public async Task<IReadOnlyList<Donation>> ListDonationsByProjectAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        this.logger.LogInformation("Retrieving donations for project {ProjectId}.", projectId);

        var donations = await this.guardianships.ListDonationsByProjectAsync(projectId, cancellationToken);

        this.logger.LogInformation("Retrieved {Count} donations for project {ProjectId}.", donations.Count, projectId);

        return donations;
    }

    // helpers

    /// <summary>
    /// Builds a localized description of the donation purpose based on the specified target value.
    /// </summary>
    /// <param name="target">The target identifier representing the type of donation purpose. Supported values include "Guardianship",
    /// "AnimalAidRequest", and "Global". If the value does not match a known type, a general purpose description is
    /// returned.</param>
    /// <returns>A string containing the localized description of the donation purpose corresponding to the specified target.
    /// Returns a general purpose description if the target is not recognized.</returns>
    private static string BuildPurpose(string target)
    {
        return target switch
        {
            "Guardianship" => "Опіка над твариною",
            "AnimalAidRequest" => "Підтримка запиту на допомогу тваринам",
            "Global" => "Пожертва на загальні потреби",
            _ => "Пожертва",
        };
    }
}

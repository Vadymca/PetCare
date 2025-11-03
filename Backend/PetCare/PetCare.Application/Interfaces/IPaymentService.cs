namespace PetCare.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetCare.Domain.Entities;

/// <summary>
/// Defines operations for recording payments and building payment return payloads.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Records a successful payment transaction and creates a corresponding donation entry.
    /// </summary>
    /// <param name="provider">The name of the payment provider that processed the transaction. Cannot be null or empty.</param>
    /// <param name="transactionId">The unique identifier of the transaction as provided by the payment provider. Cannot be null or empty.</param>
    /// <param name="amount">The amount of the donation in the specified currency. Must be a positive value.</param>
    /// <param name="currency">The ISO currency code representing the currency of the donation. Cannot be null or empty.</param>
    /// <param name="targetEntity">The type or name of the entity that is the target of the donation (e.g., campaign, project). Cannot be null or
    /// empty.</param>
    /// <param name="targetEntityId">The unique identifier of the target entity receiving the donation, if applicable.</param>
    /// <param name="recurring">Indicates whether the donation is part of a recurring payment schedule. Set to <see langword="true"/> for
    /// recurring donations; otherwise, <see langword="false"/>.</param>
    /// <param name="anonymous">Indicates whether the donation should be recorded as anonymous. Set to <see langword="true"/> to hide donor
    /// identity; otherwise, <see langword="false"/>.</param>
    /// <param name="userId">The unique identifier of the user making the donation, if available.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="Donation"/>
    /// object representing the recorded donation.</returns>
    Task<Donation> RecordChargeSuccessAsync(
        string provider,
        string transactionId,
        decimal amount,
        string currency,
        string targetEntity,
        Guid? targetEntityId,
        bool recurring,
        bool anonymous,
        Guid? userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Records a failed donation charge attempt and creates a corresponding donation entry with failure details.
    /// </summary>
    /// <param name="provider">The name of the payment provider that processed the charge attempt. Cannot be null or empty.</param>
    /// <param name="transactionId">The unique identifier of the transaction from the payment provider, if available. May be null if not provided by
    /// the provider.</param>
    /// <param name="amount">The monetary amount of the attempted donation. Must be a non-negative value.</param>
    /// <param name="currency">The ISO currency code representing the currency of the donation (e.g., "USD"). Cannot be null or empty.</param>
    /// <param name="targetEntity">The type or name of the entity that the donation was intended for (such as a campaign or organization). Cannot
    /// be null or empty.</param>
    /// <param name="targetEntityId">The unique identifier of the target entity, if available. May be null if not applicable.</param>
    /// <param name="recurring">Indicates whether the donation was intended to be a recurring payment. Set to <see langword="true"/> for
    /// recurring donations; otherwise, <see langword="false"/>.</param>
    /// <param name="anonymous">Indicates whether the donor chose to remain anonymous. Set to <see langword="true"/> if the donation is
    /// anonymous; otherwise, <see langword="false"/>.</param>
    /// <param name="userId">The unique identifier of the user who attempted the donation, if available. May be null for anonymous donations
    /// or if the user is not registered.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the donation entry reflecting the
    /// failed charge attempt.</returns>
    Task<Donation> RecordChargeFailedAsync(
        string provider,
        string? transactionId,
        decimal amount,
        string currency,
        string targetEntity,
        Guid? targetEntityId,
        bool recurring,
        bool anonymous,
        Guid? userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Builds a URL for retrieving the payment status, including the specified status and additional query parameters.
    /// </summary>
    /// <param name="basePath">The base path of the URL to which the payment status and query parameters will be appended. Must be a valid URL
    /// or URI segment.</param>
    /// <param name="status">The payment status to include in the URL, such as "pending", "completed", or "failed". This value is typically
    /// used to indicate the desired status to query.</param>
    /// <param name="data">A collection of key-value pairs representing additional query parameters to include in the URL. Keys must be
    /// non-null; values may be null to indicate an empty parameter value.</param>
    /// <returns>A string containing the constructed URL with the payment status and any additional query parameters appended.
    /// The returned URL is suitable for use in HTTP requests.</returns>
    string BuildPaymentStatusUrl(string basePath, string status, IDictionary<string, string?> data);

    /// <summary>
    /// Asynchronously retrieves a read-only list of donations made by the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose payments are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of <see
    /// cref="Donation"/> objects associated with the specified user. If the user has not made any payments, the list
    /// will be empty.</returns>
    Task<IReadOnlyList<Donation>> GetMyPaymentsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all donations ordered by most recent first.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of <see cref="Donation"/>.</returns>
    Task<IReadOnlyList<Donation>> ListAllDonationsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves donations filtered by a specific project.
    /// </summary>
    /// <param name="projectId">The unique identifier of the project.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A read-only list of <see cref="Donation"/>.</returns>
    Task<IReadOnlyList<Donation>> ListDonationsByProjectAsync(Guid projectId, CancellationToken cancellationToken = default);
}

namespace PetCare.Application.Interfaces;

using PetCare.Application.Dtos.Payments;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using System.Threading.Tasks;

/// <summary>
/// Defines a service for verifying and processing LiqPay payment callbacks asynchronously.
/// </summary>
public interface ILiqPayService
{
    /// <summary>
    /// Verifies and processes a LiqPay callback.
    /// </summary>
    /// <param name="data">Base64 data from LiqPay.</param>
    /// <param name="signature">Base64 signature.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if verification succeeded, otherwise false.</returns>
    Task<bool> ProcessCallbackAsync(string data, string signature, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new recurring payment contract for the specified user with the given amount, currency, and
    /// subscription scope.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the recurring contract is being created.</param>
    /// <param name="amount">The amount to be charged for each recurring payment. Must be a positive value.</param>
    /// <param name="currency">The three-letter ISO currency code (e.g., "USD", "EUR") in which the recurring payment will be processed. Cannot
    /// be null or empty.</param>
    /// <param name="scope">The subscription scope that defines the context or type of the recurring contract.</param>
    /// <param name="scopeId">The unique identifier of the specific scope instance, if applicable. Specify null if the contract is not tied to
    /// a particular scope instance.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a LiqPayRecurringResponseDto with
    /// details of the created recurring contract.</returns>
    Task<LiqPayRecurringResponseDto> CreateRecurringContractAsync(
        Guid userId,
        decimal amount,
        string currency,
        SubscriptionScope scope,
        Guid? scopeId,
        CancellationToken cancellationToken = default);
}

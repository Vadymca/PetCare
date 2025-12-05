namespace PetCare.Domain.Abstractions.Repositories;

using System.Threading.Tasks;
using PetCare.Domain.Aggregates;

/// <summary>
/// Defines repository operations specific to <see cref="PaymentIntent"/> aggregate.
/// </summary>
public interface IPaymentIntentRepository : IRepository<PaymentIntent>
{
    /// <summary>
    /// Retrieves a payment intent by its external provider order identifier.
    /// </summary>
    /// <param name="externalOrderId">The external order identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching intent, or null.</returns>
    Task<PaymentIntent?> FindByExternalOrderIdAsync(
        string externalOrderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a payment intent by provider payment identifier (e.g. LiqPay payment_id).
    /// </summary>
    /// <param name="providerPaymentId">The provider payment identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching intent, or null.</returns>
    Task<PaymentIntent?> FindByProviderPaymentIdAsync(
        string providerPaymentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all payment intents associated with the specified guardianship identifier.
    /// </summary>
    /// <param name="guardianshipId">The unique identifier of the guardianship for which to retrieve payment intents.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of payment intents linked to
    /// the specified guardianship. The list is empty if no payment intents are found.</returns>
    Task<List<PaymentIntent>> GetByGuardianshipIdAsync(
    Guid guardianshipId,
    CancellationToken ct = default);
}

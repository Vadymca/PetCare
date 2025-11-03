namespace PetCare.Application.Interfaces;

using System.Text.Json;
using System.Threading.Tasks;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Defines methods for interacting with the LiqPay payment service, including building checkout requests and retrieving
/// payment status information.
/// </summary>
/// <remarks>Implementations of this interface provide asynchronous operations for initiating LiqPay checkouts and
/// querying the status of orders. All methods are designed to support cancellation via a <see
/// cref="CancellationToken"/> parameter. This interface is intended for use in applications that integrate with the
/// LiqPay payment gateway.</remarks>
public interface ILiqPayClient
{
    /// <summary>
    /// Asynchronously builds a LiqPay checkout request based on the specified input parameters.
    /// </summary>
    /// <param name="input">An object containing the details required to create the LiqPay checkout, such as payment amount, currency, and
    /// customer information.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see
    /// cref="LiqPayCheckoutResponseDto"/> with the generated checkout information.</returns>
    Task<LiqPayCheckoutResponseDto> BuildCheckoutAsync(CreateLiqPayCheckoutDto input, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously retrieves the status information for the specified order.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order whose status is to be requested. Cannot be null or empty.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a JSON element with the status
    /// details of the specified order.</returns>
    Task<JsonElement> RequestStatusAsync(string orderId, CancellationToken ct = default);
}

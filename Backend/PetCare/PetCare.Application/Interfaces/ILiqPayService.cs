namespace PetCare.Application.Interfaces;

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
}

namespace PetCare.Application.Interfaces;

using System.Threading;
using System.Threading.Tasks;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Provides access to external zipcode service.
/// </summary>
public interface IZipcodebaseService
{
    /// <summary>
    /// Resolves an address by postal code using external API.
    /// </summary>
    /// <param name="postalCode">The postal code to resolve.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The resolved <see cref="Address"/> or <c>null</c> if not found.</returns>
    /// <remarks>
    /// The <paramref name="cancellationToken"/> can be used to cancel long-running requests.
    /// </remarks>
    Task<Address?> ResolveAddressAsync(string postalCode, CancellationToken cancellationToken = default);
}
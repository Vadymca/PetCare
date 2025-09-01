namespace PetCare.Application.Interfaces;

/// <summary>
/// Represents a service for generating QR codes.
/// </summary>
public interface IQrCodeGenerator
{
    /// <summary>
    /// Generates a QR code image in Base64 format from the specified URI.
    /// </summary>
    /// <param name="totpUri">The URI or text to encode into the QR code.</param>
    /// <returns>A <see cref="string"/> containing the QR code as a Base64-encoded PNG image.</returns>
    string GenerateQrCodeBase64(string totpUri);
}

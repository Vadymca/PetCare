namespace PetCare.Infrastructure.Services;

using PetCare.Application.Interfaces;
using QRCoder;

/// <summary>
/// Provides functionality to generate QR codes in Base64 format.
/// </summary>
public sealed class QrCodeGeneratorService : IQrCodeGenerator
{
    /// <summary>
    /// Generates a QR code image in Base64 format from the specified URI.
    /// </summary>
    /// <param name="totpUri">The URI or text to encode into the QR code.</param>
    /// <returns>
    /// A <see cref="string"/> containing the QR code as a Base64-encoded PNG image,
    /// suitable for embedding directly in an HTML <c>&lt;img&gt;</c> tag.
    /// </returns>
    public string GenerateQrCodeBase64(string totpUri)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(totpUri, QRCodeGenerator.ECCLevel.Q);

        using var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeBytes = qrCode.GetGraphic(20); // Повертає байти PNG

        var base64 = Convert.ToBase64String(qrCodeBytes);

        return $"data:image/png;base64,{base64}";
    }
}

namespace PetCare.Application.Interfaces;

/// <summary>
/// Provides access to email assets like logos.
/// </summary>
public interface IEmailAssetProvider
{
    /// <summary>
    /// Gets the logo as Base64 string for embedding in emails.
    /// </summary>
    /// <returns>A <see cref="string"/> containing the Logo as a Base64-encoded PNG image.</returns>
    string GetLogoBase64();
}

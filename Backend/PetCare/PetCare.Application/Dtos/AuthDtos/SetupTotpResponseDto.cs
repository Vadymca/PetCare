namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the response returned after setting up Time-based One-Time Password (TOTP) authentication for a user.
/// </summary>
/// <param name="Success">true if the TOTP setup was successful; otherwise, false.</param>
/// <param name="Message">A message describing the result of the TOTP setup operation. Typically contains error details if setup was not
/// successful.</param>
/// <param name="QrCodeImage">A base64-encoded string representing the QR code image that can be scanned by an authenticator app to configure
/// TOTP.</param>
/// <param name="ManualKey">The manual key that can be entered into an authenticator app if scanning the QR code is not possible.</param>
public sealed record SetupTotpResponseDto(
    bool Success,
    string Message,
    string QrCodeImage,
    string ManualKey);

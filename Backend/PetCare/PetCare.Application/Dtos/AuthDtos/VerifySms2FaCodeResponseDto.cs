namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of verifying an SMS-based two-factor authentication (2FA) code, including the outcome, a
/// message, and optional authentication data.
/// </summary>
/// <param name="Success">true if the 2FA code was successfully verified; otherwise, false.</param>
/// <param name="Message">A message describing the result of the verification, such as an error or success message.</param>
/// <param name="AccessToken">The access token issued upon successful verification, or null if verification failed or no token is provided.</param>
/// <param name="User">The user associated with the verified 2FA code, or null if verification was unsuccessful or user information is not
/// available.</param>
public sealed record VerifySms2FaCodeResponseDto(
    bool Success,
    string Message,
    string? AccessToken = null,
    UserDto? User = null);

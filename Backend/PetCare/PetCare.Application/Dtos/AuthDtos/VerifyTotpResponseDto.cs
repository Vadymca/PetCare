namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of a time-based one-time password (TOTP) verification attempt, including the outcome, a
/// message, and optional authentication data.
/// </summary>
/// <param name="Success">A value indicating whether the TOTP verification was successful. Set to <see langword="true"/> if the verification
/// succeeded; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message describing the result of the verification attempt. This may include error details or additional
/// information for the caller.</param>
/// <param name="AccessToken">An optional access token issued if verification is successful. This value is <see langword="null"/> if no token is
/// provided.</param>
/// <param name="User">An optional user object containing information about the authenticated user. This value is <see langword="null"/> if
/// user details are not included in the response.</param>
public record VerifyTotpResponseDto(
    bool Success,
    string Message,
    string? AccessToken = null,
    UserDto? User = null);

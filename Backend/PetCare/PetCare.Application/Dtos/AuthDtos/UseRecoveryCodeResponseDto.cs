namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of an attempt to use a recovery code for authentication.
/// </summary>
/// <param name="Success">true if the recovery code was accepted and authentication succeeded; otherwise, false.</param>
/// <param name="Message">A message describing the outcome of the recovery code attempt. May include error details or success information.</param>
/// <param name="AccessToken">The access token issued if authentication succeeds; otherwise, null.</param>
/// <param name="User">The authenticated user information if authentication succeeds; otherwise, null.</param>
public sealed record UseRecoveryCodeResponseDto(
    bool Success,
    string Message,
    string? AccessToken = null,
    UserDto? User = null);
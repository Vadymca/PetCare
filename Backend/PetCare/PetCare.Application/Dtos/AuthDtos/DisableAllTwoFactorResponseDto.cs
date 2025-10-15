namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of an operation to disable all two-factor authentication methods for a user.
/// </summary>
/// <param name="Success">true if all two-factor authentication methods were successfully disabled; otherwise, false.</param>
/// <param name="Message">A message describing the outcome of the disable operation. May contain error details or additional information.</param>
public sealed record DisableAllTwoFactorResponseDto(bool Success, string Message);

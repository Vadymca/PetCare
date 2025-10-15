namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of a request to disable SMS-based two-factor authentication (2FA).
/// </summary>
/// <param name="Success">true if the SMS 2FA was successfully disabled; otherwise, false.</param>
/// <param name="Message">A message describing the outcome of the disable operation. May contain error details or additional information.</param>
public sealed record DisableSms2FaResponseDto(bool Success, string Message);

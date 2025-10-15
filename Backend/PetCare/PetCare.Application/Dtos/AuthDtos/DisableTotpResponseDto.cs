namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of a request to disable time-based one-time password (TOTP) authentication.
/// </summary>
/// <param name="Success">A value indicating whether the TOTP disable operation was successful. Set to <see langword="true"/> if the operation
/// succeeded; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message providing additional information about the result of the TOTP disable operation. May contain error details
/// or a success confirmation.</param>
public sealed record DisableTotpResponseDto(
    bool Success,
    string Message);

namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of a TOTP (Time-based One-Time Password) setup verification attempt, including the outcome and
/// an associated message.
/// </summary>
/// <param name="Success">A value indicating whether the TOTP setup verification was successful. Set to <see langword="true"/> if the
/// verification succeeded; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message providing additional information about the verification result. This may include error details or guidance
/// for the user.</param>
public record VerifyTotpSetupResponseDto(
    bool Success,
    string Message);

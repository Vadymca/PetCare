namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of verifying an SMS-based two-factor authentication (2FA) setup attempt.
/// </summary>
/// <param name="Success">A value indicating whether the SMS 2FA setup verification was successful. Set to <see langword="true"/> if the
/// verification succeeded; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message providing additional information about the verification result. This may include error details or
/// confirmation messages.</param>
public sealed record VerifySms2FaSetupResponseDto(bool Success, string Message);

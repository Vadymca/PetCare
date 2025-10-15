namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of an attempt to send a two-factor authentication (2FA) code via SMS.
/// </summary>
/// <param name="Success">true if the SMS containing the 2FA code was sent successfully; otherwise, false.</param>
/// <param name="Message">A message describing the result of the SMS send attempt. This may include error details if the operation was not
/// successful.</param>
public sealed record SendSms2FaCodeResponseDto(bool Success, string Message);

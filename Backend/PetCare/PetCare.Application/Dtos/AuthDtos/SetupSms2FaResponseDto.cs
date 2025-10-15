namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of an attempt to set up SMS-based two-factor authentication.
/// </summary>
/// <param name="Success">A value indicating whether the SMS two-factor authentication setup was successful. Set to <see langword="true"/> if
/// the setup succeeded; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message providing additional information about the setup result, such as an error description or confirmation
/// details.</param>
public sealed record SetupSms2FaResponseDto(
     bool Success,
     string Message);

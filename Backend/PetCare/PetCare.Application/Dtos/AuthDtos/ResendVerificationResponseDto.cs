namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of a resend verification request, including the operation status and an associated message.
/// </summary>
/// <param name="Success">A value indicating whether the resend verification operation was successful. Set to <see langword="true"/> if the
/// operation succeeded; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message providing additional information about the result of the resend verification operation. May contain error
/// details or confirmation text.</param>
public sealed record ResendVerificationResponseDto(
 bool Success,
 string Message);

namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of a password reset operation, including the outcome and an associated message.
/// </summary>
/// <param name="Success">A value indicating whether the password reset operation was successful. Set to <see langword="true"/> if the
/// operation succeeded; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message providing additional information about the result of the password reset operation. This may include error
/// details or confirmation text.</param>
public sealed record ResetPasswordResponseDto(bool Success, string Message);
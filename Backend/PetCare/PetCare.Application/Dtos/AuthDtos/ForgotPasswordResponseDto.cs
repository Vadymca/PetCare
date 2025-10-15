namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of a forgot password operation, including the success status and an associated message.
/// </summary>
/// <param name="Success">A value indicating whether the forgot password request was successful. Set to <see langword="true"/> if the
/// operation succeeded; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message providing additional information about the result of the forgot password operation. This may include error
/// details or confirmation text.</param>
public sealed record ForgotPasswordResponseDto(bool Success, string Message);

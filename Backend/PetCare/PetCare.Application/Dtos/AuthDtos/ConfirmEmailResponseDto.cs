namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of an email confirmation operation, including the outcome and an associated message.
/// </summary>
/// <param name="Success">A value indicating whether the email confirmation was successful. Set to <see langword="true"/> if the confirmation
/// succeeded; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message providing additional information about the result of the email confirmation. May contain error details or
/// a success message.</param>
public record ConfirmEmailResponseDto(
bool Success,
string Message);

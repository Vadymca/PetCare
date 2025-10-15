namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the result of a logout operation, including the outcome and an optional message.
/// </summary>
/// <param name="Success">A value indicating whether the logout operation was successful. Set to <see langword="true"/> if the operation
/// succeeded; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message providing additional information about the logout operation. This may include error details or
/// confirmation text.</param>
public record LogoutResponseDto(
bool Success,
string Message);

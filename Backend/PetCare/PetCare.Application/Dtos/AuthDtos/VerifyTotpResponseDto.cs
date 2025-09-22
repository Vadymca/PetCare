namespace PetCare.Application.Dtos.AuthDtos;

public record VerifyTotpResponseDto(
    bool Success,
    string Message,
    string? AccessToken = null,
    UserDto? User = null);

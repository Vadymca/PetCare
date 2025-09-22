namespace PetCare.Application.Dtos.AuthDtos;

public sealed record UseRecoveryCodeResponseDto(
    bool Success,
    string Message,
    string? AccessToken = null,
    UserDto? User = null);
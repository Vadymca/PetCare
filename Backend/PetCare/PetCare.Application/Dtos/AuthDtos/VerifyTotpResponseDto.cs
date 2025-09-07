namespace PetCare.Application.Dtos.AuthDtos;

public record VerifyTotpResponseDto(
    bool Success,
    string Message,
    string? AccessToken,
    string? RefreshToken);

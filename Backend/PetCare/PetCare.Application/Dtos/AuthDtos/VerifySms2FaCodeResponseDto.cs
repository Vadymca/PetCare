namespace PetCare.Application.Dtos.AuthDtos;

public sealed record VerifySms2FaCodeResponseDto(
    bool Success,
    string Message,
    string? AccessToken = null,
    UserDto? User = null);

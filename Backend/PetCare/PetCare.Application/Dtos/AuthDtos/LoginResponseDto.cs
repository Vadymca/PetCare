namespace PetCare.Application.Dtos.AuthDtos;

public record LoginResponseDto(
    string AccessToken,
    string RefreshToken,
    UserDto User);

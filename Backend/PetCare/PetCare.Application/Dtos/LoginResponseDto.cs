namespace PetCare.Application.Dtos;

public record LoginResponseDto(
    string AccessToken,
    string RefreshToken,
    UserDto User);

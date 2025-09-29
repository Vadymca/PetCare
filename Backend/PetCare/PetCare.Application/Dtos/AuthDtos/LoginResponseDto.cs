namespace PetCare.Application.Dtos.AuthDtos;

public record LoginResponseDto(
    string Status,
    string? AccessToken = null,
    UserDto? User = null,
    string? Method = null,
    string? HiddenPhoneNumber = null,
    string? Message = null,
    string? TwoFaToken = null);

namespace PetCare.Application.Dtos.AuthDtos;

public record VerifyTotpSetupResponseDto(
    bool Success,
    string Message);

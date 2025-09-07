namespace PetCare.Application.Dtos.AuthDtos;

public sealed record DisableTotpResponseDto(
    bool Success,
    string Message);

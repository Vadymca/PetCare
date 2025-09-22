namespace PetCare.Application.Dtos.AuthDtos;

public sealed record SetupTotpResponseDto(
    bool Success,
    string Message,
    string QrCodeImage,
    string ManualKey);

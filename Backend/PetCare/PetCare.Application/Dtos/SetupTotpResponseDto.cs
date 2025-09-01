namespace PetCare.Application.Dtos;

public sealed record SetupTotpResponseDto(
    bool Success,
    string Message,
    string QrCodeImage,
    string ManualKey,
    string[] RecoveryCodes);

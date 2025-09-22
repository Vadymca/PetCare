namespace PetCare.Application.Dtos.AuthDtos;

public sealed record ResendVerificationResponseDto(
 bool Success,
 string Message);

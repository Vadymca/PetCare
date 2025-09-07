namespace PetCare.Application.Dtos.AuthDtos;

public record TwoFactorStatusResponseDto(
        bool IsTwoFactorEnabled,
        bool IsSms2FaEnabled);

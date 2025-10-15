namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the status of two-factor authentication and SMS-based two-factor authentication for a user account.
/// </summary>
/// <param name="IsTwoFactorEnabled">A value indicating whether two-factor authentication is enabled for the user. Set to <see langword="true"/> if
/// enabled; otherwise, <see langword="false"/>.</param>
/// <param name="IsSms2FaEnabled">A value indicating whether SMS-based two-factor authentication is enabled for the user. Set to <see
/// langword="true"/> if enabled; otherwise, <see langword="false"/>.</param>
public record TwoFactorStatusResponseDto(
        bool IsTwoFactorEnabled,
        bool IsSms2FaEnabled);

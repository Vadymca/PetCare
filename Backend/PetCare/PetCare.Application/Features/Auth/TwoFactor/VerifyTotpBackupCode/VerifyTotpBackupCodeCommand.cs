namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotpBackupCode;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to verify a TOTP (Time-based One-Time Password) backup code for two-factor authentication.
/// </summary>
/// <param name="TwoFaToken">The two-factor authentication token that identifies the user session or context. Cannot be null or empty.</param>
/// <param name="Code">The backup code to be verified. Must be a valid, unused backup code associated with the user's account.</param>
public sealed record VerifyTotpBackupCodeCommand(
    string TwoFaToken,
    string Code)
    : IRequest<VerifyTotpResponseDto>;

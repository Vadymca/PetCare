namespace PetCare.Application.Features.Auth.TwoFactor.RecoveryCodes.Use;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to redeem a two-factor authentication recovery code for account access.
/// </summary>
/// <param name="TwoFaToken">The two-factor authentication token that identifies the user session. Cannot be null or empty.</param>
/// <param name="Code">The recovery code to be used for authentication. Cannot be null or empty.</param>
public sealed record UseRecoveryCodeCommand(
    string TwoFaToken,
    string Code)
    : IRequest<UseRecoveryCodeResponseDto>;

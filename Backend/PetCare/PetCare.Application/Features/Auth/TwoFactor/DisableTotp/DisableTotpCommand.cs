namespace PetCare.Application.Features.Auth.TwoFactor.DisableTotp;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to disable time-based one-time password (TOTP) authentication for the current user.
/// </summary>
public sealed record DisableTotpCommand() : IRequest<VerifyTotpResponseDto>;

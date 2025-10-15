namespace PetCare.Application.Features.Auth.TwoFactor.GetBackupCodes;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to retrieve the current set of TOTP backup codes for the authenticated user.
/// </summary>
/// <remarks>Use this command to obtain backup codes that can be used for two-factor authentication when the
/// primary TOTP device is unavailable. The response includes all valid backup codes associated with the user at the
/// time of the request.</remarks>
public sealed record GetTotpBackupCodesCommand()
    : IRequest<GetTotpBackupCodesResponseDto>;

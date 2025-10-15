namespace PetCare.Application.Features.Auth.TwoFactor.RecoveryCodes;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to retrieve the current set of recovery codes for a user in a two-factor authentication system.
/// </summary>
/// <remarks>This command is typically used in scenarios where a user needs to view or manage their existing
/// recovery codes. Recovery codes are one-time-use codes that allow users to regain access to their account if they
/// lose access to their primary two-factor authentication method.</remarks>
public sealed record GetRecoveryCodesCommand : IRequest<RecoveryCodesResponseDto>;

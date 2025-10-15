namespace PetCare.Application.Features.Auth.TwoFactor.RegenerateBackupCodes;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to regenerate the set of backup codes for time-based one-time password (TOTP) authentication.
/// </summary>
/// <remarks>Use this command to invalidate any previously issued TOTP backup codes and generate a new set. Backup
/// codes provide an alternative means of authentication if the primary TOTP device is unavailable. This operation is
/// typically restricted to authenticated users managing their own accounts.</remarks>
public sealed record RegenerateTotpBackupCodesCommand()
    : IRequest<GetTotpBackupCodesResponseDto>;

namespace PetCare.Application.Features.Auth.TwoFactor.SetupTotp;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to initiate the setup process for Time-based One-Time Password (TOTP) multi-factor
/// authentication.
/// </summary>
/// <remarks>Use this command to begin configuring TOTP for a user account. The response typically includes
/// information required to complete TOTP setup, such as a shared secret or QR code data. This command does not perform
/// verification; it only initiates the setup process.</remarks>
public sealed record SetupTotpCommand()
    : IRequest<SetupTotpResponseDto>;

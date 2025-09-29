namespace PetCare.Application.Features.Auth.TwoFactor.RecoveryCodes.Use;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record UseRecoveryCodeCommand(
    string TwoFaToken,
    string Code)
    : IRequest<UseRecoveryCodeResponseDto>;

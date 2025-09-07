namespace PetCare.Application.Features.Auth.TwoFactor.RecoveryCodes;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record GetRecoveryCodesCommand : IRequest<RecoveryCodesResponseDto>;

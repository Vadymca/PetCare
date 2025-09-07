namespace PetCare.Application.Features.Auth.TwoFactor.GetBackupCodes;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record GetTotpBackupCodesCommand()
    : IRequest<GetTotpBackupCodesResponseDto>;

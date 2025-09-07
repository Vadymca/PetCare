namespace PetCare.Application.Features.Auth.TwoFactor.RegenerateBackupCodes;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record RegenerateTotpBackupCodesCommand()
    : IRequest<GetTotpBackupCodesResponseDto>;

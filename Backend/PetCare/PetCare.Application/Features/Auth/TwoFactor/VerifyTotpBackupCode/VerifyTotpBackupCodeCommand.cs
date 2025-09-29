namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotpBackupCode;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record VerifyTotpBackupCodeCommand(
    string TwoFaToken,
    string Code)
    : IRequest<VerifyTotpResponseDto>;

namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotp;
using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record VerifyTotpCommand(
    string TwoFaToken,
    string Code)
    : IRequest<VerifyTotpResponseDto>;

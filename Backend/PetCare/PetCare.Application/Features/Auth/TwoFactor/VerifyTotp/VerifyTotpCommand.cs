namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotp;
using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record VerifyTotpCommand(string Code)
    : IRequest<VerifyTotpResponseDto>;

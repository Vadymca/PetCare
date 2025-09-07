namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotpSetup;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record VerifyTotpSetupCommand(string Code)
    : IRequest<VerifyTotpSetupResponseDto>;

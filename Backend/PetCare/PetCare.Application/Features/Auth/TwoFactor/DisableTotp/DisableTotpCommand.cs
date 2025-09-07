namespace PetCare.Application.Features.Auth.TwoFactor.DisableTotp;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record DisableTotpCommand() : IRequest<VerifyTotpResponseDto>;

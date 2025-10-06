namespace PetCare.Application.Features.Auth.TwoFactor.SetupTotp;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record SetupTotpCommand()
    : IRequest<SetupTotpResponseDto>;

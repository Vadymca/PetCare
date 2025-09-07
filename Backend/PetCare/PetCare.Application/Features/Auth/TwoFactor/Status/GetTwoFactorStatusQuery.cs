namespace PetCare.Application.Features.Auth.TwoFactor.Status;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record GetTwoFactorStatusQuery()
    : IRequest<TwoFactorStatusResponseDto>;

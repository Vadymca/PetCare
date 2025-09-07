namespace PetCare.Application.Features.Auth.TwoFactor.DisableAll;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record DisableAllTwoFactorCommand : IRequest<DisableAllTwoFactorResponseDto>;

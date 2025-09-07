namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Disable;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record DisableSms2FaCommand() : IRequest<DisableSms2FaResponseDto>;

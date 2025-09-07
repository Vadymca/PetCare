namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Setup;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record SetupSms2FaCommand()
    : IRequest<SetupSms2FaResponseDto>;
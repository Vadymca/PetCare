namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Send;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record SendSms2FaCodeCommand(string TwoFaToken)
    : IRequest<SendSms2FaCodeResponseDto>;
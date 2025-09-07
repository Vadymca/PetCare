namespace PetCare.Application.Features.Auth.TwoFactor.Sms.VerifySetup;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record VerifySms2FaSetupCommand(string Code) : IRequest<VerifySms2FaSetupResponseDto>;

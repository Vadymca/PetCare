namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Verify;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record VerifySms2FaCodeCommand(string Code) : IRequest<VerifySms2FaCodeResponseDto>;

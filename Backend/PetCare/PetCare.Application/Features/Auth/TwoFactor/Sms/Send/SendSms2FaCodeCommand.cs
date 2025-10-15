namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Send;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to send a two-factor authentication (2FA) code via SMS using the specified 2FA token.
/// </summary>
/// <param name="TwoFaToken">The token that identifies the 2FA session for which the SMS code should be sent. Cannot be null or empty.</param>
public sealed record SendSms2FaCodeCommand(string TwoFaToken)
    : IRequest<SendSms2FaCodeResponseDto>;
namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Verify;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to verify a two-factor authentication (2FA) code sent via SMS using a provided verification
/// token.
/// </summary>
/// <param name="TwoFaToken">The token identifying the 2FA session for which the SMS code was sent. Cannot be null or empty.</param>
/// <param name="Code">The SMS verification code to be validated. Cannot be null or empty.</param>
public sealed record VerifySms2FaCodeCommand(
    string TwoFaToken,
    string Code)
    : IRequest<VerifySms2FaCodeResponseDto>;

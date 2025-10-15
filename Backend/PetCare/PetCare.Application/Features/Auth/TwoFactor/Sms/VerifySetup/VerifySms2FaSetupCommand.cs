namespace PetCare.Application.Features.Auth.TwoFactor.Sms.VerifySetup;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to verify the setup of SMS-based two-factor authentication using a verification code.
/// </summary>
/// <param name="Code">The verification code sent to the user's mobile device. Cannot be null or empty.</param>
public sealed record VerifySms2FaSetupCommand(string Code) : IRequest<VerifySms2FaSetupResponseDto>;

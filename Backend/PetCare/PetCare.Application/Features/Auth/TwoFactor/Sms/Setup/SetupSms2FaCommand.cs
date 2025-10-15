namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Setup;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to initiate the setup process for SMS-based two-factor authentication (2FA).
/// </summary>
/// <remarks>Use this command to begin the SMS 2FA enrollment workflow. The response typically includes
/// information or instructions required to complete the setup process. This command does not contain any parameters;
/// all necessary context is expected to be provided by the current user session or environment.</remarks>
public sealed record SetupSms2FaCommand()
    : IRequest<SetupSms2FaResponseDto>;
namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Disable;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to disable SMS-based two-factor authentication for the current user.
/// </summary>
public sealed record DisableSms2FaCommand() : IRequest<DisableSms2FaResponseDto>;

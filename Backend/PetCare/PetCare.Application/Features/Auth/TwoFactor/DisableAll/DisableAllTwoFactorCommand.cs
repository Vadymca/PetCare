namespace PetCare.Application.Features.Auth.TwoFactor.DisableAll;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to disable all two-factor authentication methods for a user.
/// </summary>
public sealed record DisableAllTwoFactorCommand : IRequest<DisableAllTwoFactorResponseDto>;

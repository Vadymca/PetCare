namespace PetCare.Application.Features.Auth.TwoFactor.Status;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to retrieve the current user's two-factor authentication status.
/// </summary>
/// <remarks>Use this query to determine whether two-factor authentication is enabled for the authenticated user.
/// The response provides details about the user's two-factor configuration.</remarks>
public sealed record GetTwoFactorStatusQuery()
    : IRequest<TwoFactorStatusResponseDto>;

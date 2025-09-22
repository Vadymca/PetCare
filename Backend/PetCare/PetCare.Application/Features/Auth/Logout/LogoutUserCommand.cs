namespace PetCare.Application.Features.Auth.Logout;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Command for logging out the current user.
/// </summary>
public sealed record LogoutUserCommand()
    : IRequest<LogoutResponseDto>;

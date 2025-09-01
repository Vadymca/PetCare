namespace PetCare.Application.Features.Auth.Logout;

using MediatR;

/// <summary>
/// Command for logging out the current user.
/// </summary>
public sealed record LogoutUserCommand()
    : IRequest;

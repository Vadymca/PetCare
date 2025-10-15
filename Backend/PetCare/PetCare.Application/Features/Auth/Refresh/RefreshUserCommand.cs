namespace PetCare.Application.Features.Auth.Refresh;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to refresh the current user's authentication state and obtain a new login response.
/// </summary>
/// <remarks>Use this command to trigger a refresh of the user's session or authentication tokens. The response
/// typically contains updated authentication information for the user. This command does not require any
/// parameters.</remarks>
public sealed record RefreshUserCommand()
    : IRequest<LoginResponseDto>;

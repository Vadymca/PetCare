namespace PetCare.Application.Features.Users.GetCurrentUser;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to retrieve the user information for the specified user identifier.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose information is to be retrieved.</param>
public sealed record GetCurrentUserCommand(Guid UserId) : IRequest<UserDto>;

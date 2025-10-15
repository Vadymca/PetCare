namespace PetCare.Application.Features.Users.Roles;

using MediatR;
using PetCare.Application.Dtos.UserDtos;

/// <summary>
/// Represents a request to assign a role to a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user to whom the role will be assigned.</param>
/// <param name="Role">The name of the role to assign to the user. Cannot be null or empty.</param>
public sealed record AddUserRoleCommand(
    Guid UserId,
    string Role)
    : IRequest<AddUserRoleResponseDto>;

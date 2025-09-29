namespace PetCare.Application.Features.Users.Roles;

using MediatR;
using PetCare.Application.Dtos.UserDtos;

public sealed record AddUserRoleCommand(
    Guid UserId,
    string Role)
    : IRequest<AddUserRoleResponseDto>;

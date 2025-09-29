namespace PetCare.Application.Features.Users.GetCurrentUser;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record GetCurrentUserCommand(Guid UserId) : IRequest<UserDto>;

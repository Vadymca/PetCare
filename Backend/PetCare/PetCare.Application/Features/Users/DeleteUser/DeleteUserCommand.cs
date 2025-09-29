namespace PetCare.Application.Features.Users.DeleteUser;

using MediatR;
using PetCare.Application.Dtos.UserDtos;
using System;

public sealed record DeleteUserCommand(Guid Id) : IRequest<DeleteUserResponseDto>;

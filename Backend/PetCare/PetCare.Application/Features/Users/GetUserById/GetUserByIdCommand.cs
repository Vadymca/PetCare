namespace PetCare.Application.Features.Users.GetUserById;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;
using System;

public sealed record GetUserByIdCommand(
    Guid Id)
    : IRequest<UserDto>;

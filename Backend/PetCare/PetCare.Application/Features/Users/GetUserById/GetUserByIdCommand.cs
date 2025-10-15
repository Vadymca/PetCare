namespace PetCare.Application.Features.Users.GetUserById;

using System;
using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to retrieve a user by their unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the user to retrieve.</param>
public sealed record GetUserByIdCommand(
    Guid Id)
    : IRequest<UserDto>;

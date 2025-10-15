namespace PetCare.Application.Features.Users.DeleteUser;

using System;
using MediatR;
using PetCare.Application.Dtos.UserDtos;

/// <summary>
/// Represents a request to delete a user identified by a unique identifier.
/// </summary>
/// <param name="Id">The unique identifier of the user to delete.</param>
public sealed record DeleteUserCommand(Guid Id) : IRequest<DeleteUserResponseDto>;

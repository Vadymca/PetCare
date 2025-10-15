namespace PetCare.Application.Features.Auth.Register;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Command for registering a new user.
/// </summary>
public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Phone,
    string? PostalCode)
    : IRequest<UserDto>;

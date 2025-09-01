namespace PetCare.Application.Features.Auth.Register;
using MediatR;
using PetCare.Application.Dtos;

/// <summary>
/// Command for registering a new user.
/// </summary>
public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? PostalCode)
    : IRequest<UserDto>;

namespace PetCare.Application.Features.Auth.Register;
using MediatR;
using PetCare.Application.Dtos;

/// <summary>
/// Command for registering a new user.
/// </summary>
public sealed record RegisterUserCommand(
    string email,
    string password,
    string firstName,
    string lastName,
    string phoneNumber)
    : IRequest<UserDto>;

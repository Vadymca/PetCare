namespace PetCare.Application.Features.Auth.Login;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to authenticate a user with the specified email address and password.
/// </summary>
/// <param name="Email">The email address of the user attempting to log in. Cannot be null or empty.</param>
/// <param name="Password">The password associated with the user's account. Cannot be null or empty.</param>
public sealed record LoginUserCommand(
    string Email,
    string Password)
    : IRequest<LoginResponseDto>;
namespace PetCare.Application.Features.Auth.ForgotPassword;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to initiate the password reset process for a user identified by email address.
/// </summary>
/// <param name="Email">The email address of the user requesting a password reset. Cannot be null or empty.</param>
public sealed record ForgotPasswordCommand(string Email)
    : IRequest<ForgotPasswordResponseDto>;

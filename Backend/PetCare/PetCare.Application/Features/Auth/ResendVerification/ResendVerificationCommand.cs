namespace PetCare.Application.Features.Auth.ResendVerification;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to resend a verification email to a user identified by their email address.
/// </summary>
/// <param name="Email">The email address of the user to whom the verification email will be resent. Cannot be null or empty.</param>
public sealed record ResendVerificationCommand(string Email)
    : IRequest<ResendVerificationResponseDto>;

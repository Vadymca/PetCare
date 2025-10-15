namespace PetCare.Application.Features.Auth.ResetPassword;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents a request to reset a user's password using a verification token.
/// </summary>
/// <param name="Email">The email address of the user whose password is to be reset. Cannot be null or empty.</param>
/// <param name="Token">The password reset verification token associated with the user. Cannot be null or empty.</param>
/// <param name="NewPassword">The new password to set for the user. Must meet the application's password requirements.</param>
public sealed record ResetPasswordCommand(
 string Email,
 string Token,
 string NewPassword)
    : IRequest<ResetPasswordResponseDto>;

namespace PetCare.Application.Features.Auth.Facebook.FacebookLogin;

using MediatR;

/// <summary>
/// Represents a command containing the authorization code and state returned from a Facebook login callback.
/// </summary>
/// <param name="Code">The authorization code received from Facebook after a successful user login. This code is used to obtain an access
/// token.</param>
/// <param name="State">The state parameter returned by Facebook to help prevent cross-site request forgery (CSRF) attacks. This value
/// should match the original state sent in the login request.</param>
public sealed record FacebookLoginCallbackCommand(
    string Code,
    string State)
    : IRequest<string>;

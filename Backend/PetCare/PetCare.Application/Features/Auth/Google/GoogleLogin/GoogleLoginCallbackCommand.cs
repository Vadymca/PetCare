namespace PetCare.Application.Features.Auth.Google.GoogleLogin;

using MediatR;

/// <summary>
/// Represents a command containing the authorization code and state returned from a Google login callback.
/// </summary>
/// <param name="Code">The authorization code received from Google after a successful user authentication. This value is used to exchange
/// for access and ID tokens. Cannot be null or empty.</param>
/// <param name="State">The state parameter returned by Google to help prevent cross-site request forgery (CSRF) attacks. This value should
/// match the original state sent in the authentication request. Cannot be null or empty.</param>
public sealed record GoogleLoginCallbackCommand(
    string Code,
    string State)
    : IRequest<string>;

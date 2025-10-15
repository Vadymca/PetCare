namespace PetCare.Application.Features.Auth.Google.GetGoogleLoginUrl;

using MediatR;

/// <summary>
/// Represents a request to generate a Google OAuth login URL with the specified state parameter.
/// </summary>
/// <param name="State">A value used to maintain state between the authentication request and the callback. Typically used to prevent
/// cross-site request forgery (CSRF) attacks or to carry application-specific data. Cannot be null.</param>
public sealed record GetGoogleLoginUrlCommand(
    string State)
    : IRequest<string>;

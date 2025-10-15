namespace PetCare.Application.Features.Auth.Facebook.GetFacebookLoginUrl;

using MediatR;

/// <summary>
/// Represents a request to generate a Facebook login URL with the specified state parameter.
/// </summary>
/// <param name="State">A value used to maintain state between the request and callback. Typically used to prevent cross-site request
/// forgery (CSRF) attacks. Cannot be null.</param>
public sealed record GetFacebookLoginUrlCommand(string State)
    : IRequest<string>;

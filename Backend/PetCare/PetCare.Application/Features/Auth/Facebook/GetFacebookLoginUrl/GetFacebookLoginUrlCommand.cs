namespace PetCare.Application.Features.Auth.Facebook.GetFacebookLoginUrl;

using MediatR;

public sealed record GetFacebookLoginUrlCommand(string State)
    : IRequest<string>;

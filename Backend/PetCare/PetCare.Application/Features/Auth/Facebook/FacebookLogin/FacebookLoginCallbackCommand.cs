namespace PetCare.Application.Features.Auth.Facebook.FacebookLogin;

using MediatR;

public sealed record FacebookLoginCallbackCommand(
    string Code,
    string State)
    : IRequest<string>;

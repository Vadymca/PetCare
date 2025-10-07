namespace PetCare.Application.Features.Auth.Google.GoogleLogin;

using MediatR;

public sealed record GoogleLoginCallbackCommand(
    string Code,
    string State)
    : IRequest<string>;

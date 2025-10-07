namespace PetCare.Application.Features.Auth.Google.GetGoogleLoginUrl;

using MediatR;

public sealed record GetGoogleLoginUrlCommand(
    string State)
    : IRequest<string>;

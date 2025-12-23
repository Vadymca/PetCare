namespace PetCare.Application.Features.Auth.Social.ExchangeMiniToken;

using MediatR;

/// <summary>
/// Command for exchanging a mini token for authentication cookies.
/// </summary>
public sealed record ExchangeMiniTokenCommand(string Token) : IRequest;

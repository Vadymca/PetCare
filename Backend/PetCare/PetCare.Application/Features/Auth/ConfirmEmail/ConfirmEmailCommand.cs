namespace PetCare.Application.Features.Auth.ConfirmEmail;
using MediatR;

/// <summary>
/// Command for confirming user email.
/// </summary>
public sealed record ConfirmEmailCommand(
    string Email,
    string Token)
    : IRequest<bool>;

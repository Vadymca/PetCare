namespace PetCare.Application.Features.Auth.ResendVerification;

using MediatR;

public sealed record ResendVerificationCommand(string Email)
    : IRequest<bool>;

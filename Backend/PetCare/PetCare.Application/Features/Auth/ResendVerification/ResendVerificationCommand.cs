namespace PetCare.Application.Features.Auth.ResendVerification;

using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record ResendVerificationCommand(string Email)
    : IRequest<ResendVerificationResponseDto>;

namespace PetCare.Application.Features.Auth.ForgotPassword;
using MediatR;
using PetCare.Application.Dtos;

public sealed record ForgotPasswordCommand(string Email)
    : IRequest<ForgotPasswordResponseDto>;

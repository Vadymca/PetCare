namespace PetCare.Application.Features.Auth.ForgotPassword;
using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record ForgotPasswordCommand(string Email)
    : IRequest<ForgotPasswordResponseDto>;

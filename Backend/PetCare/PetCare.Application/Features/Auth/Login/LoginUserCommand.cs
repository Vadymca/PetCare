namespace PetCare.Application.Features.Auth.Login;
using MediatR;
using PetCare.Application.Dtos;

public sealed record LoginUserCommand(
    string Email,
    string Password)
    : IRequest<LoginResponseDto>;
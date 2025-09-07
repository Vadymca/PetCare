namespace PetCare.Application.Features.Auth.Refresh;
using MediatR;
using PetCare.Application.Dtos.AuthDtos;

public sealed record RefreshUserCommand()
    : IRequest<LoginResponseDto>;

namespace PetCare.Application.Features.Auth.Refresh;
using MediatR;
using PetCare.Application.Dtos;

public sealed record RefreshUserCommand()
    : IRequest<LoginResponseDto>;

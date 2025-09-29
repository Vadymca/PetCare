namespace PetCare.Application.Features.Users.GetUserActivity;

using MediatR;
using PetCare.Application.Dtos.UserDtos;
using System;

public sealed record GetUserActivityCommand(
    Guid UserId)
    : IRequest<GetUserActivityResponseDto>;

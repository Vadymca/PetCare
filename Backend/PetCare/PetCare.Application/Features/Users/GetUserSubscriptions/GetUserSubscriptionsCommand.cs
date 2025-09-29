namespace PetCare.Application.Features.Users.GetUserSubscriptions;

using MediatR;
using PetCare.Application.Dtos.UserDtos;
using System;

public sealed record GetUserSubscriptionsCommand(
    Guid UserId)
    : IRequest<GetUserSubscriptionsResponseDto>;

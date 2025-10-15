namespace PetCare.Application.Features.Users.GetUserSubscriptions;

using System;
using MediatR;
using PetCare.Application.Dtos.UserDtos;

/// <summary>
/// Represents a request to retrieve all subscriptions associated with a specific user.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose subscriptions are to be retrieved.</param>
public sealed record GetUserSubscriptionsCommand(
    Guid UserId)
    : IRequest<GetUserSubscriptionsResponseDto>;

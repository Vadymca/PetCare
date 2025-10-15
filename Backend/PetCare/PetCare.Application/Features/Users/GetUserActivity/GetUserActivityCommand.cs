namespace PetCare.Application.Features.Users.GetUserActivity;

using System;
using MediatR;
using PetCare.Application.Dtos.UserDtos;

/// <summary>
/// Represents a request to retrieve activity information for a specific user.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose activity is to be retrieved.</param>
public sealed record GetUserActivityCommand(
    Guid UserId)
    : IRequest<GetUserActivityResponseDto>;

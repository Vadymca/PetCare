namespace PetCare.Application.Features.Shelters.GetFavoriteShelters;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;

/// <summary>
/// Represents a request to retrieve the list of favorite shelters for a specified user.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose favorite shelters are to be retrieved.</param>
public sealed record GetFavoriteSheltersCommand(Guid UserId)
    : IRequest<IReadOnlyList<ShelterListDto>>;

namespace PetCare.Application.Dtos.UserDtos;

using System.Collections.Generic;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Dtos.ShelterDtos;

/// <summary>
/// Represents the response data containing the list of shelters and animals a user is subscribed to.
/// </summary>
/// <param name="Shelters">The collection of shelters to which the user is currently subscribed. Cannot be null.</param>
/// <param name="Animals">The collection of animals to which the user is currently subscribed. Cannot be null.</param>
public sealed record GetUserSubscriptionsResponseDto(
 IReadOnlyList<ShelterListDto> Shelters,
 IReadOnlyList<AnimalListDto> Animals);

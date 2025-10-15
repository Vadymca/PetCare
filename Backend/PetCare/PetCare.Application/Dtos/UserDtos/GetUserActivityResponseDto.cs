namespace PetCare.Application.Dtos.UserDtos;

using System.Collections.Generic;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Application.Dtos.EventDtos;

/// <summary>
/// Represents the response data containing a user's adoption applications and event participation details.
/// </summary>
/// <param name="AdoptionApplications">A read-only list of adoption application records associated with the user. Cannot be null.</param>
/// <param name="Events">A read-only list of event records in which the user has participated. Cannot be null.</param>
public sealed record GetUserActivityResponseDto(
 IReadOnlyList<AdoptionApplicationDto> AdoptionApplications,
 IReadOnlyList<EventDto> Events);

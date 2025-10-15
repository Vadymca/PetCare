namespace PetCare.Application.Dtos.AnimalDtos;

using System.Collections.Generic;

/// <summary>
/// Represents the response returned when retrieving a list of animals, including the collection of animals and the
/// total number of animals available.
/// </summary>
/// <param name="Animals">The collection of animals included in the current response. The list may be empty if no animals match the query.</param>
/// <param name="TotalCount">The total number of animals available that match the query criteria, regardless of paging or filtering applied to
/// the response.</param>
public sealed record GetAnimalsResponseDto(
     IReadOnlyList<AnimalListDto> Animals,
     int TotalCount);

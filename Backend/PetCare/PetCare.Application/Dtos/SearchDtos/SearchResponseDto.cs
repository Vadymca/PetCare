namespace PetCare.Application.Dtos.SearchDtos;

using System.Collections.Generic;

/// <summary>
/// Represents the full search response grouped by entity type.
/// </summary>
public sealed record SearchResponseDto(
    IReadOnlyList<SearchResultItemDto> Animals,
    IReadOnlyList<SearchResultItemDto> Shelters,
    IReadOnlyList<SearchResultItemDto> Projects,
    IReadOnlyList<SearchResultItemDto> News,
    IReadOnlyList<SearchResultItemDto> Stories,
    IReadOnlyList<SearchResultItemDto> Pages)
{
    /// <summary>
    /// Gets an empty instance of the <see cref="SearchResponseDto"/> class.
    /// </summary>
    /// <remarks>Use this property to represent a response with no results or data. The returned instance
    /// contains empty collections for all properties.</remarks>
    public static SearchResponseDto Empty =>
        new([], [], [], [], [], []);
}
namespace PetCare.Application.Dtos.SearchDtos;

/// <summary>
/// Represents a single search result item.
/// </summary>
public sealed record SearchResultItemDto(
    string Title,
    string Slug,
    string? Snippet);

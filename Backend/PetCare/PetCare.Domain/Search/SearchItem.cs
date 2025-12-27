namespace PetCare.Domain.Search;

/// <summary>
/// Lightweight domain read model for search results.
/// </summary>
public sealed record SearchItem(
    string Name,
    string Slug,
    string? Snippet);

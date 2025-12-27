namespace PetCare.Application.Features.Search;

using MediatR;
using PetCare.Application.Dtos.SearchDtos;

/// <summary>
/// Command for performing a global search.
/// </summary>
/// <param name="Query">Search query entered by the user.</param>
/// <param name="Language">Optional language code (e.g., "uk", "en"). Defaults to "uk".</param>
public sealed record GlobalSearchCommand(
    string Query,
    string? Language = "uk") : IRequest<SearchResponseDto>;
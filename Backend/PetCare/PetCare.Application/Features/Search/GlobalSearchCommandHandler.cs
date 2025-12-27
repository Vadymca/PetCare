namespace PetCare.Application.Features.Search;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.SearchDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for global search command.
/// </summary>
public sealed class GlobalSearchCommandHandler
    : IRequestHandler<GlobalSearchCommand, SearchResponseDto>
{
    private readonly ISearchService searchService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalSearchCommandHandler"/> class.
    /// </summary>
    /// <param name="searchService">The search service.</param>
    public GlobalSearchCommandHandler(ISearchService searchService)
    {
        this.searchService = searchService
            ?? throw new ArgumentNullException(nameof(searchService));
    }

    /// <inheritdoc />
    public async Task<SearchResponseDto> Handle(
        GlobalSearchCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
        {
            return SearchResponseDto.Empty;
        }

        // 🇺🇦 мова за замовчуванням
        var language = string.IsNullOrWhiteSpace(request.Language)
            ? "uk"
            : request.Language;

        return await this.searchService.SearchAsync(
            request.Query,
            language,
            cancellationToken);
    }
}

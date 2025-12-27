namespace PetCare.Api.Endpoints.Search;

using MediatR;
using PetCare.Application.Dtos.SearchDtos;
using PetCare.Application.Features.Search;

/// <summary>
/// Maps the POST /api/search/global endpoint.
/// </summary>
public static class GlobalSearchEndpoint
{
    /// <summary>
    /// Maps the POST /api/search/global endpoint for performing a global search.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to add the endpoint to.</param>
    public static void MapGlobalSearchEndpoint(this WebApplication app)
    {
        app.MapPost("/api/search", async (
                IMediator mediator,
                GlobalSearchCommand command,
                ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GlobalSearchEndpoint");
            logger.LogInformation("Global search request: {Query}", command.Query);

            var response = await mediator.Send(command);

            if (response != null)
            {
                logger.LogInformation(
                    "Global search completed: {Count} results",
                    response.Animals.Count + response.Shelters.Count + response.Projects.Count + response.News.Count + response.Stories.Count + response.Pages.Count);

                return Results.Ok(response);
            }

            logger.LogWarning("Global search returned no results for query: {Query}", command.Query);
            return Results.Ok(new SearchResponseDto(
                Animals: Array.Empty<SearchResultItemDto>(),
                Shelters: Array.Empty<SearchResultItemDto>(),
                Projects: Array.Empty<SearchResultItemDto>(),
                News: Array.Empty<SearchResultItemDto>(),
                Stories: Array.Empty<SearchResultItemDto>(),
                Pages: Array.Empty<SearchResultItemDto>()));
        })
            .WithName("GlobalSearch")
            .RequireRateLimiting("GlobalPolicy")
            .WithTags("Search")
            .Produces<SearchResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError)
            .Accepts<GlobalSearchCommand>("application/json");
    }
}

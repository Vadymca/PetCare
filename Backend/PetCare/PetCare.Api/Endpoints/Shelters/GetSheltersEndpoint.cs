namespace PetCare.Api.Endpoints.Shelters;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Features.Shelters.GetShelters;

/// <summary>
/// Configures the endpoint for retrieving a paginated list of shelters.
/// </summary>
public static class GetSheltersEndpoint
{
    /// <summary>
    /// Maps the GET endpoint for retrieving a paginated list of shelters to the specified web application.
    /// </summary>
    /// <remarks>The endpoint is accessible at '/api/shelters' and returns shelter data in a paginated format.
    /// The endpoint supports optional query parameters for page number and page size. The response is returned with a
    /// 200 OK status code and includes a <see cref="GetSheltersResponseDto"/> object.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to which the GET shelters endpoint will be added.</param>
    public static void MapGetSheltersEndpoint(this WebApplication app)
    {
        app.MapGet("/api/shelters", async (
            IMediator mediator,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20) =>
        {
            var command = new GetSheltersCommand(page, pageSize);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("GetShelters")
        .WithTags("Shelters")
        .RequireRateLimiting("GlobalPolicy")
        .Produces<GetSheltersResponseDto>(StatusCodes.Status200OK);
    }
}

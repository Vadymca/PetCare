namespace PetCare.Api.Endpoints.Auth;

using MediatR;
using PetCare.Application.Features.Auth.Logout;

/// <summary>
/// Contains the endpoint mapping for user logout.
/// </summary>
public static class LogoutEndpoint
{
    /// <summary>
    /// Maps the POST /api/auth/logout endpoint to handle user logout requests.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure endpoints on.</param>
    public static void MapLogoutEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/logout", async (HttpContext context, IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            try
            {
                var logger = loggerFactory.CreateLogger("LogoutEndpoint");

                // Читаємо JSON з body (якщо потрібно, зараз можна без тіла)
                using var reader = new StreamReader(context.Request.Body);
                var json = await reader.ReadToEndAsync();
                logger.LogInformation("Received raw JSON for logout: {Json}", string.IsNullOrWhiteSpace(json) ? "<empty>" : json);

                var command = new LogoutUserCommand();

                await mediator.Send(command);

                return Results.Ok(new { message = "Користувач вийшов, cookies очищено." });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireRateLimiting("GlobalPolicy")
        .WithName("Logout")
        .WithTags("Auth")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError)
        .Accepts<object>("application/json");
    }
}

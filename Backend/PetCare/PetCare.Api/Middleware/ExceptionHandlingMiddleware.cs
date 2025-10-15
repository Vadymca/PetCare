namespace PetCare.Api.Middleware;

using System.Net;
using System.Text.Json;

/// <summary>
/// Middleware for handling exceptions globally.
/// </summary>
public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (FluentValidation.ValidationException ex) // validation errors
        {
            this.logger.LogWarning(ex, "Validation failed");
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray());

            var payload = new
            {
                error = "Валідація не пройдена",
                details = errors,
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
        catch (JsonException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Некоректний формат JSON",
                details = ex.Message,
            });
        }
        catch (InvalidOperationException ex) // business rule violation
        {
            this.logger.LogWarning(ex, "Business rule violation");
            await this.WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (ArgumentException ex) // validation error
        {
            this.logger.LogWarning(ex, "Validation error");
            await this.WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (KeyNotFoundException ex) // resource not found
        {
            this.logger.LogWarning(ex, "Resource not found");
            await this.WriteErrorAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (UnauthorizedAccessException ex) // auth issue
        {
            this.logger.LogWarning(ex, "Unauthorized access");
            await this.WriteErrorAsync(context, HttpStatusCode.Forbidden, ex.Message);
        }
        catch (Exception ex) // unexpected
        {
            this.logger.LogError(ex, "Unexpected error");
            await this.WriteErrorAsync(context, HttpStatusCode.InternalServerError, "Виникла внутрішня помилка. Спробуйте пізніше.");
        }
    }

    private async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = new { error = message };
        var json = JsonSerializer.Serialize(payload);

        await context.Response.WriteAsync(json);
    }
}

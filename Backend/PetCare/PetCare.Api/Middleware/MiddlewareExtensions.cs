namespace PetCare.Api.Middleware;

public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds global exception handling middleware.
    /// </summary>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}

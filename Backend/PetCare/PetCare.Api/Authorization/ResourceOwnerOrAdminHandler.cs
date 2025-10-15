namespace PetCare.Api.Authorization;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Authorization handler to enforce the "ResourceOwnerOrAdmin" policy.
/// Grants access if the current user is an Admin or the owner of the resource.
/// </summary>
public sealed class ResourceOwnerOrAdminHandler : AuthorizationHandler<ResourceOwnerOrAdminRequirement>
{
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceOwnerOrAdminHandler"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">Accessor to the current HTTP context.</param>
    public ResourceOwnerOrAdminHandler(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Handles the authorization requirement.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <param name="requirement">The requirement to evaluate.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ResourceOwnerOrAdminRequirement requirement)
    {
        var httpContext = this.httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        // Отримуємо Id користувача з токена
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? context.User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        // Перевірка на Admin
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Перевірка власника ресурсу через параметр {id} маршруту
        var routeId = httpContext.Request.RouteValues["id"]?.ToString();
        if (routeId == userIdClaim)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        context.Fail();
        return Task.CompletedTask;
    }
}

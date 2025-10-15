namespace PetCare.Application.Features.Auth.Logout;

using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Domain.Abstractions.Services;

/// <summary>
/// Handles <see cref="LogoutUserCommand"/> request.
/// Deletes JWT cookies for the current user.
/// </summary>
public sealed class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, LogoutResponseDto>
{
    private readonly IJwtService jwtService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<LogoutUserCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogoutUserCommandHandler"/> class.
    /// </summary>
    /// <param name="jwtService">Service for handling JWT operations.</param>
    /// <param name="httpContextAccessor">Accessor for the current HTTP context.</param>
    /// <param name="logger">Logger instance for diagnostic messages.</param>
    public LogoutUserCommandHandler(
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<LogoutUserCommandHandler> logger)
    {
        this.jwtService = jwtService;
        this.httpContextAccessor = httpContextAccessor;
        this.logger = logger;
    }

    /// <summary>
    /// Handles the <see cref="LogoutUserCommand"/> request.
    /// Clears JWT cookies from the response to log out the current user.
    /// </summary>
    /// <param name="request">The logout command request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A completed task once the cookies are cleared.</returns>
    /// A <see cref="LogoutResponseDto"/> indicating whether the logout was successful.
    /// </returns>
    public Task<LogoutResponseDto> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var response = this.httpContextAccessor.HttpContext!.Response;

        this.jwtService.ClearCookies(response);

        this.logger.LogInformation("Користувач вийшов, JWT cookies очищено.");

        return Task.FromResult(new LogoutResponseDto(
            Success: true,
            Message: "Ви успішно вийшли з системи."));
    }
}

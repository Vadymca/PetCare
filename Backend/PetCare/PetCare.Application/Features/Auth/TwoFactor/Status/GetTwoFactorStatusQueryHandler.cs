namespace PetCare.Application.Features.Auth.TwoFactor.Status;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the retrieval of current user's 2FA status.
/// </summary>
public sealed class GetTwoFactorStatusQueryHandler : IRequestHandler<GetTwoFactorStatusQuery, TwoFactorStatusResponseDto>
{
    private readonly IUserService userService;
    private readonly ILogger<GetTwoFactorStatusQueryHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTwoFactorStatusQueryHandler"/> class.
    /// </summary>
    /// <param name="userService">Service to access user information.</param>
    /// <param name="logger">Logger instance for tracking operations.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="userService"/> or <paramref name="logger"/> is null.</exception>
    public GetTwoFactorStatusQueryHandler(IUserService userService, ILogger<GetTwoFactorStatusQueryHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the query to retrieve the current user's 2FA status.
    /// </summary>
    /// <param name="request">The 2FA status query request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="TwoFactorStatusResponseDto"/> containing:
    /// <list type="bullet">
    /// <item><description><c>IsTwoFactorEnabled</c> — overall 2FA enabled flag.</description></item>
    /// <item><description><c>IsSms2FaEnabled</c> — SMS 2FA enabled flag.</description></item>
    /// </list>
    /// </returns>
    public async Task<TwoFactorStatusResponseDto> Handle(GetTwoFactorStatusQuery request, CancellationToken cancellationToken)
    {
        var user = await this.userService.GetCurrentUserAsync();
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to get 2FA status.");
            throw new InvalidOperationException("Користувач не авторизований.");
        }

        var status = this.userService.GetTwoFactorStatus(user);
        this.logger.LogInformation("Retrieved 2FA status for user {UserId}", user.Id);
        return status;
    }
}

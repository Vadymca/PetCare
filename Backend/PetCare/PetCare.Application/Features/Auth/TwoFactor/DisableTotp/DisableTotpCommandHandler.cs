namespace PetCare.Application.Features.Auth.TwoFactor.DisableTotp;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the disabling of TOTP (two-factor authentication) for the current user.
/// </summary>
public sealed class DisableTotpCommandHandler : IRequestHandler<DisableTotpCommand, VerifyTotpResponseDto>
{
    private readonly IUserService userService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILogger<DisableTotpCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DisableTotpCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used to retrieve and manage user data.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor used to access the current HTTP request and user.</param>
    /// <param name="logger">The logger used to log information and errors.</param>
    public DisableTotpCommandHandler(
        IUserService userService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<DisableTotpCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<VerifyTotpResponseDto> Handle(DisableTotpCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.GetCurrentUserAsync();
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to disable TOTP.");
            throw new InvalidOperationException("Користувач не авторизований.");
        }

        await this.userService.DisableTotpAsync(user);

        this.logger.LogInformation("2FA successfully disabled for user {Email}", user.Email);
        return new VerifyTotpResponseDto(
            Success: true,
            Message: "Двофакторну аутентифікацію вимкнено успішно.");
    }
}

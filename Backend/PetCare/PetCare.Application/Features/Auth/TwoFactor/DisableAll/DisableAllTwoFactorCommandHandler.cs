namespace PetCare.Application.Features.Auth.TwoFactor.DisableAll;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the disabling of all 2FA methods for the current user.
/// </summary>
public sealed class DisableAllTwoFactorCommandHandler : IRequestHandler<DisableAllTwoFactorCommand, DisableAllTwoFactorResponseDto>
{
    private readonly IUserService userService;
    private readonly ILogger<DisableAllTwoFactorCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DisableAllTwoFactorCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used to manage user operations.</param>
    /// <param name="logger">The logger used for logging information and warnings.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="userService"/> or <paramref name="logger"/> is <c>null</c>.
    /// </exception>
    public DisableAllTwoFactorCommandHandler(
        IUserService userService,
        ILogger<DisableAllTwoFactorCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<DisableAllTwoFactorResponseDto> Handle(DisableAllTwoFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.GetCurrentUserAsync();
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to disable all 2FA methods.");
            throw new InvalidOperationException("Користувач не авторизований.");
        }

        user.TwoFactorEnabled = false;
        user.PhoneNumberConfirmed = false;

        var result = await this.userService.DisableAllTwoFactorAsync(user);
        if (!result)
        {
            this.logger.LogWarning("Failed to disable all 2FA methods for user {UserId}", user.Id);
            throw new InvalidOperationException("Не вдалося відключити всі методи 2FA.");
        }

        this.logger.LogInformation("All 2FA methods disabled for user {UserId}", user.Id);
        return new DisableAllTwoFactorResponseDto(
            Success: true,
            Message: "Усі методи 2FA успішно відключено.");
    }
}

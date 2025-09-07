namespace PetCare.Application.Features.Auth.TwoFactor.RecoveryCodes.Use;

using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using System;
using System.Threading.Tasks;

/// <summary>
/// Handles the usage of a recovery code for two-factor authentication.
/// </summary>
public sealed class UseRecoveryCodeCommandHandler : IRequestHandler<UseRecoveryCodeCommand, UseRecoveryCodeResponseDto>
{
    private readonly IUserService userService;
    private readonly ILogger<UseRecoveryCodeCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UseRecoveryCodeCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used to manage user operations and 2FA functionality.</param>
    /// <param name="logger">The logger used to log information, warnings, and errors.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="userService"/> or <paramref name="logger"/> is null.
    /// </exception>
    public UseRecoveryCodeCommandHandler(
        IUserService userService,
        ILogger<UseRecoveryCodeCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<UseRecoveryCodeResponseDto> Handle(UseRecoveryCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.GetCurrentUserAsync();
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to use recovery code.");
            return new UseRecoveryCodeResponseDto(false, "Користувач не авторизований.");
        }

        var success = await this.userService.RedeemRecoveryCodeAsync(user, request.Code);
        if (!success)
        {
            return new UseRecoveryCodeResponseDto(false, "Невірний або вже використаний код відновлення.");
        }

        return new UseRecoveryCodeResponseDto(true, "Код відновлення прийнято.");
    }
}

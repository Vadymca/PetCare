namespace PetCare.Application.Features.Auth.TwoFactor.GetBackupCodes;

using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using System;
using System.Threading.Tasks;

/// <summary>
/// Handles retrieval of TOTP backup codes for the currently authenticated user.
/// </summary>
public sealed class GetTotpBackupCodesCommandHandler
    : IRequestHandler<GetTotpBackupCodesCommand, GetTotpBackupCodesResponseDto>
{
    private readonly IUserService userService;
    private readonly ILogger<GetTotpBackupCodesCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTotpBackupCodesCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used to retrieve user information.</param>
    /// <param name="logger">Logger for tracking operations.</param>
    public GetTotpBackupCodesCommandHandler(
        IUserService userService,
        ILogger<GetTotpBackupCodesCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<GetTotpBackupCodesResponseDto> Handle(
        GetTotpBackupCodesCommand request,
        CancellationToken cancellationToken)
    {
        // Отримуємо поточного користувача
        var user = await this.userService.GetCurrentUserAsync();
        if (user is null)
        {
            this.logger.LogWarning("Unauthorized attempt to retrieve TOTP backup codes.");
            return new GetTotpBackupCodesResponseDto(
                Success: false,
                Message: "Користувач не авторизований.",
                BackupCodes: null);
        }

        // Перевіряємо, чи увімкнено 2FA
        if (!user.TwoFactorEnabled)
        {
            return new GetTotpBackupCodesResponseDto(
                Success: false,
                Message: "Двофакторна аутентифікація не активована.",
                BackupCodes: null);
        }

        // Отримуємо резервні коди через сервіс користувача
        var backupCodes = await this.userService.GetTotpBackupCodesAsync(user);

        this.logger.LogInformation("TOTP backup codes retrieved for user {Email}", user.Email);

        return new GetTotpBackupCodesResponseDto(
            Success: true,
            Message: "Резервні коди отримано успішно.",
            BackupCodes: backupCodes);
    }
}

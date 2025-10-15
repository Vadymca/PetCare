namespace PetCare.Application.Features.Auth.TwoFactor.SetupTotp;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles TOTP setup by generating shared key, QR code, and recovery codes
/// for the authenticated user, using UserService.
/// </summary>
public sealed class SetupTotpCommandHandler : IRequestHandler<SetupTotpCommand, SetupTotpResponseDto>
{
    private readonly IUserService userService;
    private readonly IQrCodeGenerator qrCodeGenerator;
    private readonly ILogger<SetupTotpCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetupTotpCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    /// <param name="qrCodeGenerator">QR code generator for TOTP URIs.</param>
    /// <param name="logger">Logger instance for diagnostics.</param>
    public SetupTotpCommandHandler(
        IUserService userService,
        IQrCodeGenerator qrCodeGenerator,
        ILogger<SetupTotpCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.qrCodeGenerator = qrCodeGenerator ?? throw new ArgumentNullException(nameof(qrCodeGenerator));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<SetupTotpResponseDto> Handle(SetupTotpCommand request, CancellationToken cancellationToken)
    {
        // Отримуємо поточного користувача
        var user = await this.userService.GetCurrentUserAsync()
                   ?? throw new InvalidOperationException("Не вдалося визначити користувача.");

        // Отримуємо або генеруємо ключ TOTP
        var unformattedKey = await this.userService.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrWhiteSpace(unformattedKey))
        {
            this.logger.LogInformation("No TOTP key found, generating new key for user {UserId}", user.Id);
            unformattedKey = await this.userService.ResetAuthenticatorKeyAsync(user)
                            ?? throw new InvalidOperationException("Не вдалося згенерувати TOTP ключ.");
        }

        // Безпечне отримання email
        var email = await this.userService.GetEmailAsync(user);
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidOperationException("Не вдалося отримати email користувача.");
        }

        // Форматування ключа та генерація QR-коду
        var sharedKey = FormatKey(unformattedKey);
        var authenticatorUri = GenerateQrCodeUri(email, unformattedKey);
        var qrCodeImage = this.qrCodeGenerator.GenerateQrCodeBase64(authenticatorUri);

        // Генеруємо recovery-коди
        var recoveryCodes = await this.userService.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        this.logger.LogInformation("TOTP setup generated successfully for user {UserId}", user.Id);

        return new SetupTotpResponseDto(
            Success: true,
            Message: "TOTP секрет згенеровано успішно.",
            QrCodeImage: qrCodeImage,
            ManualKey: sharedKey);
    }

    private static string FormatKey(string unformattedKey)
    {
        if (string.IsNullOrWhiteSpace(unformattedKey))
        {
            return string.Empty;
        }

        return string.Join(" ", Enumerable.Range(0, unformattedKey.Length / 4)
                                         .Select(i => unformattedKey.Substring(i * 4, 4)))
                     .ToLowerInvariant();
    }

    private static string GenerateQrCodeUri(string email, string unformattedKey)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(unformattedKey))
        {
            return string.Empty;
        }

        return $"otpauth://totp/PetCare:{email}?secret={unformattedKey}&issuer=PetCare&digits=6";
    }
}
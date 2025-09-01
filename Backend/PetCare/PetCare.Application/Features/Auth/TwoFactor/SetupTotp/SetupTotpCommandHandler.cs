namespace PetCare.Application.Features.Auth.TwoFactor.SetupTotp;

using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos;
using PetCare.Application.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Handles TOTP setup by generating shared key, QR code, and recovery codes
/// for the authenticated user, using UserService.
/// </summary>
public sealed class SetupTotpCommandHandler : IRequestHandler<SetupTotpCommand, SetupTotpResponseDto>
{
    private readonly IUserService userService;
    private readonly IQrCodeGenerator qrCodeGenerator;
    private readonly ILogger<SetupTotpCommandHandler> logger;

    public SetupTotpCommandHandler(
        IUserService userService,
        IQrCodeGenerator qrCodeGenerator,
        ILogger<SetupTotpCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.qrCodeGenerator = qrCodeGenerator ?? throw new ArgumentNullException(nameof(qrCodeGenerator));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SetupTotpResponseDto> Handle(SetupTotpCommand request, CancellationToken cancellationToken)
    {
        // Отримуємо поточного користувача
        var user = await userService.GetCurrentUserAsync();
        if (user == null)
        {
            logger.LogWarning("Unable to identify user for TOTP setup.");
            return new SetupTotpResponseDto(
                Success: false,
                Message: "Не вдалося визначити користувача.",
                QrCodeImage: string.Empty,
                ManualKey: string.Empty,
                RecoveryCodes: Array.Empty<string>()
            );
        }

        // Отримуємо або генеруємо ключ TOTP
        var unformattedKey = await userService.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrWhiteSpace(unformattedKey))
        {
            logger.LogInformation("No TOTP key found, generating new key for user {UserId}", user.Id);
            unformattedKey = await userService.ResetAuthenticatorKeyAsync(user);
        }

        if (string.IsNullOrWhiteSpace(unformattedKey))
        {
            logger.LogError("TOTP key generation failed for user {UserId}", user.Id);
            return new SetupTotpResponseDto(
                Success: false,
                Message: "Не вдалося згенерувати TOTP ключ.",
                QrCodeImage: string.Empty,
                ManualKey: string.Empty,
                RecoveryCodes: Array.Empty<string>()
            );
        }

        // Безпечне отримання email
        var email = await userService.GetEmailAsync(user);
        if (string.IsNullOrWhiteSpace(email))
        {
            logger.LogError("User email is empty for user {UserId}", user.Id);
            return new SetupTotpResponseDto(
                Success: false,
                Message: "Не вдалося отримати email користувача.",
                QrCodeImage: string.Empty,
                ManualKey: string.Empty,
                RecoveryCodes: Array.Empty<string>()
            );
        }

        // Форматування ключа та генерація QR-коду
        var sharedKey = FormatKey(unformattedKey);
        var authenticatorUri = GenerateQrCodeUri(email, unformattedKey);
        var qrCodeImage = qrCodeGenerator.GenerateQrCodeBase64(authenticatorUri);

        // Генеруємо recovery-коди
        var recoveryCodes = await userService.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        logger.LogInformation("TOTP setup generated successfully for user {UserId}", user.Id);

        return new SetupTotpResponseDto(
            Success: true,
            Message: "TOTP секрет згенеровано успішно.",
            QrCodeImage: qrCodeImage,
            ManualKey: sharedKey,
            RecoveryCodes: recoveryCodes);
    }

    private static string FormatKey(string unformattedKey)
    {
        if (string.IsNullOrWhiteSpace(unformattedKey))
            return string.Empty;

        return string.Join(" ", Enumerable.Range(0, unformattedKey.Length / 4)
                                         .Select(i => unformattedKey.Substring(i * 4, 4)))
                     .ToLowerInvariant();
    }

    private static string GenerateQrCodeUri(string email, string unformattedKey)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(unformattedKey))
            return string.Empty;

        return $"otpauth://totp/PetCare:{email}?secret={unformattedKey}&issuer=PetCare&digits=6";
    }
}
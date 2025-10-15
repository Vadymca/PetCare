namespace PetCare.Application.Features.Auth.TwoFactor.VerifyTotpSetup;

using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles verification of TOTP setup for the currently authenticated user.
/// </summary>
public sealed class VerifyTotpSetupCommandHandler
    : IRequestHandler<VerifyTotpSetupCommand, VerifyTotpSetupResponseDto>
{
    private readonly IUserService userService;
    private readonly ILogger<VerifyTotpSetupCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyTotpSetupCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    /// <param name="logger">Logger for tracking operations.</param>
    public VerifyTotpSetupCommandHandler(
        IUserService userService,
        ILogger<VerifyTotpSetupCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the verification of a TOTP code for a user's two-factor authentication setup.
    /// </summary>
    /// <param name="request">The command containing the TOTP code.</param>
    /// <param name="cancellationToken">Token for canceling the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="VerifyTotpSetupResponseDto"/> indicating success or failure
    /// and an appropriate message.
    /// </returns>
    public async Task<VerifyTotpSetupResponseDto> Handle(
        VerifyTotpSetupCommand request,
        CancellationToken cancellationToken)
    {
        // Отримуємо поточного авторизованого користувача
        var user = await this.userService.GetCurrentUserAsync();
        if (user is null)
        {
            this.logger.LogWarning("Unauthorized attempt to verify TOTP setup.");
            throw new UnauthorizedAccessException("Користувач не авторизований.");
        }

        // Перевіряємо TOTP код
        var isValid = await this.userService.VerifyTotpCodeAsync(user, request.Code);
        if (!isValid)
        {
            throw new InvalidOperationException("Невірний код.");
        }

        // Активуємо двофакторну автентифікацію
        await this.userService.EnableTwoFactorAsync(user);

        this.logger.LogInformation("2FA successfully enabled for user {Email}", user.Email);

        return new VerifyTotpSetupResponseDto(
            Success: true,
            Message: "Двофакторна автентифікація успішно активована.");
    }
}

namespace PetCare.Application.Features.Auth.TwoFactor.Sms.VerifySetup;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles verification of SMS 2FA setup code for the current user.
/// </summary>
public sealed class VerifySms2FaSetupCommandHandler
    : IRequestHandler<VerifySms2FaSetupCommand, VerifySms2FaSetupResponseDto>
{
    private readonly IUserService userService;
    private readonly ISms2FaService sms2FaService;
    private readonly ILogger<VerifySms2FaSetupCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerifySms2FaSetupCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">Service for managing user data and operations.</param>
    /// <param name="sms2FaService">Service for handling SMS 2FA operations.</param>
    /// <param name="logger">Logger for logging information and errors.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any of the required dependencies are null.
    /// </exception>
    public VerifySms2FaSetupCommandHandler(
        IUserService userService,
        ISms2FaService sms2FaService,
        ILogger<VerifySms2FaSetupCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.sms2FaService = sms2FaService ?? throw new ArgumentNullException(nameof(sms2FaService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<VerifySms2FaSetupResponseDto> Handle(
        VerifySms2FaSetupCommand request,
        CancellationToken cancellationToken)
    {
        var user = await this.userService.GetCurrentUserAsync();
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to verify SMS 2FA setup code.");
            throw new UnauthorizedAccessException("Користувач не авторизований.");
        }

        // Перевіряємо код через сервіс SMS 2FA
        var isValid = await this.sms2FaService.VerifySetupCodeAsync(user.Id.ToString(), request.Code);
        if (!isValid)
        {
            this.logger.LogWarning("Invalid SMS 2FA setup code attempt for user {UserId}", user.Id);
            throw new InvalidOperationException("Невірний код підтвердження.");
        }

        // Підтверджуємо номер телефону користувача
        await this.userService.ConfirmPhoneNumberAsync(user);

        this.logger.LogInformation("SMS 2FA setup verified and phone confirmed for user {UserId}", user.Id);

        return new VerifySms2FaSetupResponseDto(true, "Номер телефону успішно підтверджено. SMS 2FA активована.");
    }
}

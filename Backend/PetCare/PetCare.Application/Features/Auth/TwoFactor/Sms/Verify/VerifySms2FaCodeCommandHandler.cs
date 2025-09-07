namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Verify;

using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using System;
using System.Threading.Tasks;

/// <summary>
/// Handles verification of the SMS 2FA code for the current user.
/// </summary>
public sealed class VerifySms2FaCodeCommandHandler : IRequestHandler<VerifySms2FaCodeCommand, VerifySms2FaCodeResponseDto>
{
    private readonly IUserService userService;
    private readonly ISms2FaService sms2FaService;
    private readonly ILogger<VerifySms2FaCodeCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerifySms2FaCodeCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">Service to manage user operations.</param>
    /// <param name="sms2FaService">Service to handle SMS 2FA operations.</param>
    /// <param name="logger">Logger instance.</param>
    public VerifySms2FaCodeCommandHandler(
        IUserService userService,
        ISms2FaService sms2FaService,
        ILogger<VerifySms2FaCodeCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.sms2FaService = sms2FaService ?? throw new ArgumentNullException(nameof(sms2FaService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<VerifySms2FaCodeResponseDto> Handle(VerifySms2FaCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.GetCurrentUserAsync();
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to verify SMS 2FA code.");
            return new VerifySms2FaCodeResponseDto(false, "Користувач не авторизований.");
        }

        var verified = await this.sms2FaService.VerifySetupCodeAsync(user.Id.ToString(), request.Code);
        if (!verified)
        {
            this.logger.LogWarning("Invalid SMS 2FA code attempt for user {UserId}", user.Id);
            return new VerifySms2FaCodeResponseDto(false, "Невірний код.");
        }

        if (!user.PhoneNumberConfirmed)
        {
            await this.userService.ConfirmPhoneNumberAsync(user);
        }

        this.logger.LogInformation("SMS 2FA code successfully verified for user {UserId}", user.Id);
        return new VerifySms2FaCodeResponseDto(true, "SMS 2FA код успішно верифіковано.");
    }
}

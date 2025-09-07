namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Setup;

using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using System;
using System.Threading.Tasks;

/// <summary>
/// Handles the setup of SMS 2FA for the current user.
/// </summary>
public sealed class SetupSms2FaCommandHandler : IRequestHandler<SetupSms2FaCommand, SetupSms2FaResponseDto>
{
    private readonly IUserService userService;
    private readonly ISms2FaService sms2FaService;
    private readonly ILogger<SetupSms2FaCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetupSms2FaCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The service for retrieving user information.</param>
    /// <param name="sms2FaService">The service responsible for SMS 2FA operations.</param>
    /// <param name="logger">The logger instance for diagnostics and operational logs.</param>
    /// <exception cref="ArgumentNullException">Thrown if any of the required dependencies are null.</exception>
    public SetupSms2FaCommandHandler(
        IUserService userService,
        ISms2FaService sms2FaService,
        ILogger<SetupSms2FaCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.sms2FaService = sms2FaService ?? throw new ArgumentNullException(nameof(sms2FaService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<SetupSms2FaResponseDto> Handle(SetupSms2FaCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.GetCurrentUserAsync();
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to setup SMS 2FA.");
            return new SetupSms2FaResponseDto(false, "Користувач не авторизований.");
        }

        if (string.IsNullOrWhiteSpace(user.Phone))
        {
            return new SetupSms2FaResponseDto(false, "Номер телефону не вказаний у профілі.");
        }

        // Відправляємо SMS-код на номер користувача
        var sent = await this.sms2FaService.SendSetupCodeAsync(user.Id.ToString(), user.Phone);
        if (!sent)
        {
            return new SetupSms2FaResponseDto(false, "Не вдалося відправити SMS. Спробуйте пізніше.");
        }

        var maskedPhone = MaskPhoneNumber(user.Phone);
        this.logger.LogInformation("SMS 2FA setup code sent to user {UserId} at {PhoneNumber}", user.Id, maskedPhone);

        return new SetupSms2FaResponseDto(true, $"SMS 2FA код відправлено успішно на номер {maskedPhone}.");
    }

    /// <summary>
    /// Masks the phone number for logging and response.
    /// Example: +380502223209 -> +380*******09.
    /// </summary>
    private static string MaskPhoneNumber(string phone)
    {
        if (string.IsNullOrEmpty(phone) || phone.Length < 6)
        {
            return phone;
        }

        // Дістаємо код країни (+380)
        var countryCode = phone.Substring(0, 4);
        var last2 = phone[^2..];

        // Маскуємо середню частину
        var maskedMiddle = new string('*', phone.Length - countryCode.Length - last2.Length);

        return $"{countryCode}{maskedMiddle}{last2}";
    }
}

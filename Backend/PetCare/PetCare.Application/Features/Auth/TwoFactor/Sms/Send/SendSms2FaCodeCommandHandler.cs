namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Send;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles sending SMS 2FA code to the current user.
/// </summary>
public sealed class SendSms2FaCodeCommandHandler : IRequestHandler<SendSms2FaCodeCommand, SendSms2FaCodeResponseDto>
{
    private readonly IUserService userService;
    private readonly ISms2FaService sms2FaService;
    private readonly ILogger<SendSms2FaCodeCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendSms2FaCodeCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">Service to get information about the current user.</param>
    /// <param name="sms2FaService">Service to generate and send SMS 2FA codes.</param>
    /// <param name="logger">Logger for monitoring events and errors.</param>
    public SendSms2FaCodeCommandHandler(
        IUserService userService,
        ISms2FaService sms2FaService,
        ILogger<SendSms2FaCodeCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.sms2FaService = sms2FaService ?? throw new ArgumentNullException(nameof(sms2FaService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the command to send SMS 2FA code.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Response indicating success or failure.</returns>
    public async Task<SendSms2FaCodeResponseDto> Handle(SendSms2FaCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.GetUserByTwoFaTokenAsync(request.TwoFaToken)
                    ?? throw new UnauthorizedAccessException("Користувач не авторизований.");

        if (string.IsNullOrWhiteSpace(user.Phone))
        {
            throw new InvalidOperationException("Номер телефону не вказаний у профілі.");
        }

        var sent = await this.sms2FaService.SendSetupCodeAsync(user.Id.ToString(), user.Phone);
        if (!sent)
        {
            throw new InvalidOperationException("Не вдалося відправити SMS. Спробуйте пізніше.");
        }

        this.logger.LogInformation("SMS 2FA code sent to user {UserId} at {PhoneNumber}", user.Id, MaskPhoneNumber(user.Phone));
        return new SendSms2FaCodeResponseDto(
            Success: true,
            Message: $"SMS 2FA код відправлено на номер {MaskPhoneNumber(user.Phone)}.");
    }

    private static string MaskPhoneNumber(string phone)
    {
        if (string.IsNullOrEmpty(phone) || phone.Length < 7)
        {
            return phone;
        }

        var last2 = phone[^2..];
        var countryCode = phone.StartsWith("+380") ? "+380" : phone[..4];
        return $"{countryCode}*******{last2}";
    }
}

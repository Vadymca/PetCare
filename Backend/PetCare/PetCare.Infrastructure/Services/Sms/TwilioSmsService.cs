namespace PetCare.Infrastructure.Services.Sms;

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetCare.Application.Interfaces;
using PetCare.Infrastructure.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

/// <summary>
/// SMS service implementation using Twilio as the provider.
/// Responsible for sending SMS messages to users.
/// </summary>
public sealed class TwilioSmsService : ISmsService
{
    private readonly TwilioSettings twilio;
    private readonly SmsSettings sms;
    private readonly ILogger<TwilioSmsService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TwilioSmsService"/> class.
    /// Sets up the Twilio client with account credentials.
    /// </summary>
    /// <param name="twilioOptions">Twilio configuration options (AccountSid, AuthToken, etc.).</param>
    /// <param name="smsOptions">General SMS configuration (app name, expiration time, etc.).</param>
    /// <param name="logger">The logger instance for diagnostic and operational messages.</param>
    public TwilioSmsService(
        IOptions<TwilioSettings> twilioOptions,
        IOptions<SmsSettings> smsOptions,
        ILogger<TwilioSmsService> logger)
    {
        this.twilio = twilioOptions.Value;
        this.sms = smsOptions.Value;
        this.logger = logger;

        TwilioClient.Init(this.twilio.AccountSid, this.twilio.AuthToken);
    }

    /// <summary>
    /// Sends an SMS message to the specified phone number using Twilio API.
    /// </summary>
    /// <param name="toPhoneE164">
    /// The recipient phone number in E.164 format (e.g., +380501234567).
    /// </param>
    /// <param name="message">The message body to be sent.</param>
    /// <param name="cancellationToken">Optional cancellation token for the async operation.</param>
    /// <returns>
    /// A boolean value indicating whether the message was successfully sent (<c>true</c>) or failed (<c>false</c>).
    /// </returns>
    public async Task<bool> SendAsync(string toPhoneE164, string message, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await MessageResource.CreateAsync(
                body: $"[{this.sms.ApplicationName}] {message}",
                from: new PhoneNumber(this.twilio.FromPhoneNumber),
                to: new PhoneNumber(toPhoneE164));

            if (this.twilio.EnableLogging)
            {
                this.logger.LogInformation("SMS sent to {To}. Status: {Status}; Sid: {Sid}", toPhoneE164, result.Status, result.Sid);
            }

            return true;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to send SMS to {To}", toPhoneE164);
            return false;
        }
    }
}

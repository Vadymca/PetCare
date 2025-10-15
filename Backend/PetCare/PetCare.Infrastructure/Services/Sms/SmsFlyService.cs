namespace PetCare.Infrastructure.Services.Sms;

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetCare.Application.Interfaces;

/// <summary>
/// SMS service implementation using SmsFly as the provider.
/// Responsible for sending SMS messages to users via SmsFly API.
/// </summary>
public class SmsFlyService : ISmsService
{
    private readonly HttpClient httpClient;
    private readonly SmsFlySettings settings;
    private readonly ILogger<SmsFlyService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SmsFlyService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client instance for API requests.</param>
    /// <param name="settings">The SmsFly configuration options.</param>
    /// <param name="logger">The logger instance for diagnostic and operational messages.</param>
    public SmsFlyService(HttpClient httpClient, IOptions<SmsFlySettings> settings, ILogger<SmsFlyService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        logger.LogDebug(
            "SmsFlyService initialized. BaseUrl: {BaseUrl}, Sender: {Sender}",
            this.settings.BaseUrl,
            this.settings.Sender);
    }

    /// <summary>
    /// Sends an SMS message to the specified phone number using SmsFly API.
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
        if (string.IsNullOrWhiteSpace(toPhoneE164) || string.IsNullOrWhiteSpace(message))
        {
            this.logger.LogError(
                "Invalid input: phone={Phone}, messageLength={Length}",
                toPhoneE164,
                message?.Length ?? 0);
            return false;
        }

        try
        {
            var phoneNumber = NormalizePhoneNumber(toPhoneE164);
            if (!IsValidPhoneNumber(phoneNumber))
            {
                this.logger.LogError("Invalid phone number: {Phone}", phoneNumber);
                return false;
            }

            var balance = await this.CheckBalanceAsync(cancellationToken);
            if (balance <= 0)
            {
                this.logger.LogWarning("Low balance: {Balance:F2} UAH", balance);
            }

            var requestData = new
            {
                auth = new { key = this.settings.ApiKey },
                action = "SENDMESSAGE",
                data = new
                {
                    recipient = phoneNumber,
                    channels = new[] { "sms" },
                    sms = new
                    {
                        source = this.settings.Sender,
                        ttl = 300,
                        flash = 0,
                        text = message,
                    },
                },
            };

            var response = await this.httpClient.PostAsJsonAsync(this.settings.BaseUrl, requestData, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogError("HTTP {StatusCode}: {Content}", response.StatusCode, content);
                return false;
            }

            // Проста перевірка успіху через JsonDocument
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            var success = (root.TryGetProperty("success", out var successProp) && successProp.GetInt32() == 1) ||
                         (root.TryGetProperty("status", out var statusProp) && statusProp.GetString()?.Equals("ok", StringComparison.OrdinalIgnoreCase) == true);

            if (success)
            {
                this.logger.LogInformation("SMS sent to {Phone}", MaskPhoneNumber(phoneNumber));
            }
            else if (root.TryGetProperty("error", out var errorObj) &&
                     errorObj.TryGetProperty("code", out var errorCode) &&
                     errorObj.TryGetProperty("description", out var errorDesc))
            {
                this.logger.LogError(
                    "SMS failed. Error: {Code} - {Description}",
                    errorCode.GetString(),
                    errorDesc.GetString());
            }
            else
            {
                this.logger.LogError("SMS failed. Response: {Content}", content);
            }

            return success;
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogError(ex, "HTTP request failed to {Phone}", toPhoneE164);
            return false;
        }
        catch (TaskCanceledException ex)
        {
            this.logger.LogError(ex, "Request cancelled/timeout for {Phone}", toPhoneE164);
            return false;
        }
        catch (JsonException ex)
        {
            this.logger.LogError(ex, "JSON parsing failed for {Phone}", toPhoneE164);
            return false;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error sending SMS to {Phone}", toPhoneE164);
            return false;
        }
    }

    /// <summary>
    /// Normalizes phone number to international format for Ukrainian numbers.
    /// </summary>
    /// <param name="phoneNumber">The input phone number.</param>
    /// <returns>The normalized phone number or empty string if invalid.</returns>
    private static string NormalizePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return string.Empty;
        }

        var normalized = System.Text.RegularExpressions.Regex.Replace(phoneNumber, @"[^\d+]", string.Empty);

        if (normalized.StartsWith("+"))
        {
            normalized = normalized[1..];
        }

        // Ukrainian number normalization
        if (normalized.StartsWith("0") && normalized.Length == 10)
        {
            return "380" + normalized[1..];
        }

        if (normalized.Length == 9 && normalized.All(char.IsDigit))
        {
            return "380" + normalized;
        }

        if (normalized.Length == 12 && normalized.StartsWith("380"))
        {
            return normalized;
        }

        return normalized;
    }

    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) || !phoneNumber.All(char.IsDigit))
        {
            return false;
        }

        if (phoneNumber.Length != 12 || !phoneNumber.StartsWith("380"))
        {
            return false;
        }

        var operatorCode = phoneNumber[3..5];
        var validOperators = new[] { "50", "63", "66", "67", "68", "73", "93", "95", "96", "97", "98", "99" };
        return validOperators.Contains(operatorCode);
    }

    private static string MaskPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length < 6)
        {
            return phoneNumber;
        }

        var masked = phoneNumber.StartsWith("+") ? phoneNumber[1..] : phoneNumber;
        if (masked.Length < 6)
        {
            return phoneNumber;
        }

        var countryCode = masked[..3];
        var lastDigits = masked[^3..];
        var middleLength = masked.Length - countryCode.Length - lastDigits.Length;
        var maskedMiddle = middleLength > 0 ? new string('*', middleLength) : string.Empty;

        return $"+{countryCode}{maskedMiddle}{lastDigits}";
    }

    private async Task<decimal> CheckBalanceAsync(CancellationToken cancellationToken)
    {
        try
        {
            var request = new { auth = new { key = this.settings.ApiKey }, action = "BALANCE" };
            var response = await this.httpClient.PostAsJsonAsync(this.settings.BaseUrl, request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return 0;
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            // Спроба парсингу JSON
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            if (root.TryGetProperty("balance", out var balanceProp) &&
                balanceProp.ValueKind == JsonValueKind.Number &&
                balanceProp.TryGetDecimal(out var balance))
            {
                return balance;
            }

            // Фолбек на простий текст
            return decimal.TryParse(
                content?.Trim(),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out var textBalance) ? textBalance : 0;
        }
        catch (Exception ex)
        {
            this.logger.LogWarning(ex, "Failed to check balance");
            return 0;
        }
    }
}
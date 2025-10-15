namespace PetCare.Infrastructure.Services.Sms;

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;

/// <summary>
/// Provides functionality to manage SMS-based two-factor authentication (2FA) setup for users.
/// Uses <see cref="ISmsService"/> to send SMS and <see cref="IMemoryCache"/> to store temporary verification codes.
/// </summary>
public sealed class Sms2FaService : ISms2FaService
{
    private readonly ISmsService smsService;
    private readonly IMemoryCache cache;
    private readonly ILogger<Sms2FaService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Sms2FaService"/> class.
    /// </summary>
    /// <param name="smsService">The SMS sending service.</param>
    /// <param name="cache">The memory cache used to store verification codes temporarily.</param>
    /// <param name="logger">The logger for logging information and errors.</param>
    public Sms2FaService(ISmsService smsService, IMemoryCache cache, ILogger<Sms2FaService> logger)
    {
        this.smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Generates a verification code and sends it via SMS to the specified phone number.
    /// The code is cached temporarily for later verification.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="phoneNumber">The phone number to which the code should be sent.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> representing the asynchronous operation.
    /// Returns <c>true</c> if the SMS was successfully sent; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> SendSetupCodeAsync(string userId, string phoneNumber)
    {
        // Генеруємо випадковий код
        var code = new Random().Next(100000, 999999).ToString();

        // Зберігаємо код у кеші на 5 хвилин
        this.cache.Set(this.GetCacheKey(userId), code, TimeSpan.FromMinutes(5));

        var result = await this.smsService.SendAsync(
            phoneNumber,
            $"Ваш код підтвердження для входу: {code}. Він дійсний протягом 5 хвилин.");

        if (result)
        {
            this.logger.LogInformation("SMS 2FA code sent to {PhoneNumber} for user {UserId}", phoneNumber, userId);
        }

        return result;
    }

    /// <summary>
    /// Verifies the code entered by the user against the cached value.
    /// Removes the code from cache after successful verification.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="code">The verification code provided by the user.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> representing the asynchronous operation.
    /// Returns <c>true</c> if the code is valid; otherwise, <c>false</c>.
    /// </returns>
    public Task<bool> VerifySetupCodeAsync(string userId, string code)
    {
        if (this.cache.TryGetValue(this.GetCacheKey(userId), out string? cachedCode))
        {
            if (cachedCode == code)
            {
                // Видаляємо код після успішної перевірки
                this.cache.Remove(this.GetCacheKey(userId));
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }

    private string GetCacheKey(string userId) => $"sms2fa_setup_{userId}";
}

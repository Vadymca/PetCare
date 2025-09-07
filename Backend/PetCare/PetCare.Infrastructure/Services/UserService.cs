namespace PetCare.Infrastructure.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OtpNet;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Service for user management operations using ASP.NET Core Identity.
/// </summary>
public sealed class UserService : IUserService
{
    private readonly UserManager<User> userManager;
    private readonly AppDbContext dbContext;
    private readonly ILogger<UserService> logger;
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userManager">The user manager used to perform user-related operations.</param>
    /// <param name="dbContext">The database context used to save domain events and persist changes.</param>
    /// <param name="logger">The logger instance used to record diagnostic and operational messages.</param>
    /// <param name="httpContextAccessor">Provides access to the current HTTP context.</param>
    public UserService(
        UserManager<User> userManager,
        AppDbContext dbContext,
        ILogger<UserService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <summary>
    /// Creates a new user with the specified information and adds a domain event.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="firstName">The user's first name.</param>
    /// <param name="lastName">The user's last name.</param>
    /// <param name="phoneNumber">The user's phone number.</param>
    /// <param name="postalCode">
    /// Optional postal code (ZIP) of the user's address. Can be <c>null</c> if not provided.
    /// </param>
    /// <returns>The created <see cref="User"/> entity.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the user creation fails with validation errors.
    /// </exception>
    public async Task<User> CreateUserAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        string phoneNumber,
        string? postalCode)
    {
        this.logger.LogInformation("Creating user with email: {Email}", email);

        var user = User.Create(
            email: email,
            firstName: firstName,
            lastName: lastName,
            phone: phoneNumber,
            role: UserRole.User,
            postalCode: postalCode);

        this.logger.LogInformation("User ID before CreateAsync: {UserId}", user.Id);

        var result = await this.userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            this.logger.LogError("Failed to create user {Email}: {Errors}", email, errors);
            throw new InvalidOperationException($"Не вдалося створити користувача: {errors}");
        }

        this.logger.LogInformation("User ID after CreateAsync: {UserId}", user.Id);

        // After UserManager.CreateAsync, the user object should have the correct ID
        // Add the domain event with the correct ID
        user.AddUserCreatedEvent();
        this.logger.LogInformation("UserCreatedEvent added to user {UserId}", user.Id);

        // Manually trigger SaveChangesAsync to publish domain events
        // UserManager doesn't go through our AppDbContext.SaveChangesAsync override
        await this.dbContext.SaveChangesAsync();

        user.ClearDomainEvents();

        this.logger.LogInformation("User created successfully: {UserId}", user.Id);
        return user;
    }

    /// <summary>
    /// Finds a user by email.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The <see cref="User"/> entity if found; otherwise, <c>null</c>.</returns>
    public async Task<User?> FindByEmailAsync(string email)
    {
        return await this.userManager.FindByEmailAsync(email);
    }

    /// <summary>
    /// Finds a user by ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The <see cref="User"/> entity if found; otherwise, <c>null</c>.</returns>
    public async Task<User?> FindByIdAsync(Guid userId)
    {
        return await this.userManager.FindByIdAsync(userId.ToString());
    }

    /// <summary>
    /// Generates an email confirmation token for the specified user.
    /// </summary>
    /// <param name="user">The user entity.</param>
    /// <returns>The email confirmation token as a string.</returns>
    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        return await this.userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    /// <summary>
    /// Confirms a user's email using the specified confirmation token.
    /// </summary>
    /// <param name="user">The user entity.</param>
    /// <param name="token">The email confirmation token.</param>
    /// <returns><c>true</c> if the email was successfully confirmed; otherwise, <c>false</c>.</returns>
    public async Task<bool> ConfirmEmailAsync(User user, string token)
    {
        var result = await this.userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
        {
            user.AddEmailConfirmedEvent();

            await this.dbContext.SaveChangesAsync();

            user.ClearDomainEvents();

            this.logger.LogInformation("Email confirmed for user {UserId}", user.Id);
            return true;
        }

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        this.logger.LogWarning("Failed to confirm email for user {UserId}. Errors: {Errors}", user.Id, errors);
        return false;
    }

    /// <summary>
    /// Checks whether the specified password is valid for the given user.
    /// </summary>
    /// <param name="user">The user entity.</param>
    /// <param name="password">The password to check.</param>
    /// <returns><c>true</c> if the password is valid; otherwise, <c>false</c>.</returns>
    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await this.userManager.CheckPasswordAsync(user, password);
    }

    /// <summary>
    /// Retrieves the roles assigned to the specified user.
    /// </summary>
    /// <param name="user">The user entity.</param>
    /// <returns>A list of role names assigned to the user.</returns>
    public async Task<IList<string>> GetRolesAsync(User user)
    {
        return await this.userManager.GetRolesAsync(user);
    }

    /// <summary>
    /// Resets the password for the specified user using the provided reset token.
    /// Updates the user's password hash and triggers the corresponding domain event.
    /// </summary>
    /// <param name="user">The user whose password is to be reset.</param>
    /// <param name="token">The password reset token.</param>
    /// <param name="newPassword">The new password to set.</param>
    /// <returns>
    /// A <see cref="Task{Boolean}"/> representing the asynchronous operation.
    /// Returns <c>true</c> if the password was successfully reset; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> ResetPasswordAsync(User user, string token, string newPassword)
    {
        var tokenValid = await this.userManager.VerifyUserTokenAsync(
            user,
            this.userManager.Options.Tokens.PasswordResetTokenProvider,
            "ResetPassword",
            token);

        if (!tokenValid)
        {
            this.logger.LogWarning("Invalid password reset token for user {Email}", user.Email);
            return false;
        }

        var newPasswordHash = this.userManager.PasswordHasher.HashPassword(user, newPassword);

        user.ChangePassword(newPasswordHash, user.Id);

        await this.dbContext.SaveChangesAsync();

        user.ClearDomainEvents();

        this.logger.LogInformation("Password successfully reset for {Email}", user.Email);
        return true;
    }

    /// <summary>
    /// Generates a password reset token for the specified user.
    /// This token can be sent via email to allow the user to reset their password.
    /// </summary>
    /// <param name="user">The user for whom to generate the reset token.</param>
    /// <returns>
    /// A <see cref="Task{String}"/> representing the asynchronous operation,
    /// containing the generated password reset token.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="user"/> is null.</exception>
    public async Task<string> GeneratePasswordResetTokenAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return await this.userManager.GeneratePasswordResetTokenAsync(user);
    }

    /// <summary>
    /// Gets the currently authenticated user based on the HTTP context.
    /// </summary>
    /// <returns>The current <see cref="User"/> if authenticated; otherwise, <c>null</c>.</returns>
    public async Task<User?> GetCurrentUserAsync()
    {
        var httpContext = this.httpContextAccessor.HttpContext;
        if (httpContext == null || httpContext.User?.Identity == null || !httpContext.User.Identity.IsAuthenticated)
        {
            return null;
        }

        var user = await this.userManager.GetUserAsync(httpContext.User);
        return user;
    }

    /// <summary>
    /// Gets the email address of the specified user.
    /// </summary>
    /// <param name="user">The user whose email to retrieve.</param>
    /// <returns>The email address if available; otherwise, an empty string.</returns>
    public async Task<string> GetEmailAsync(User user)
    {
        return await this.userManager.GetEmailAsync(user) ?? string.Empty;
    }

    /// <summary>
    /// Gets the TOTP authenticator key for the specified user.
    /// </summary>
    /// <param name="user">The user whose authenticator key to retrieve.</param>
    /// <returns>The authenticator key if available; otherwise, <c>null</c>.</returns>
    public async Task<string?> GetAuthenticatorKeyAsync(User user)
    {
        return await this.userManager.GetAuthenticatorKeyAsync(user);
    }

    /// <summary>
    /// Resets and generates a new TOTP authenticator key for the specified user.
    /// </summary>
    /// <param name="user">The user whose authenticator key to reset.</param>
    /// <returns>The newly generated authenticator key.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a new authenticator key could not be generated.</exception>
    public async Task<string> ResetAuthenticatorKeyAsync(User user)
    {
        await this.userManager.ResetAuthenticatorKeyAsync(user);
        var key = await this.userManager.GetAuthenticatorKeyAsync(user);
        return key ?? throw new InvalidOperationException("Не вдалося згенерувати ключ TOTP.");
    }

    /// <summary>
    /// Generates new recovery codes for two-factor authentication for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate recovery codes.</param>
    /// <param name="count">The number of recovery codes to generate.</param>
    /// <returns>An array of newly generated recovery codes.</returns>
    public async Task<string[]> GenerateNewTwoFactorRecoveryCodesAsync(User user, int count)
    {
        var codes = await this.userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, count);
        return codes?.ToArray() ?? Array.Empty<string>();
    }

    /// <summary>
    /// Verifies a TOTP (Time-based One-Time Password) code for the specified user.
    /// </summary>
    /// <param name="user">The user whose TOTP code is being verified.</param>
    /// <param name="code">The TOTP code provided by the user.</param>
    /// <returns>
    /// <c>true</c> if the code is valid for the current TOTP window; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> VerifyTotpCodeAsync(User user, string code)
    {
        var secret = await this.userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(secret))
        {
            return false;
        }

        var totp = new Totp(Base32Encoding.ToBytes(secret));
        return totp.VerifyTotp(code, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
    }

    /// <summary>
    /// Enables two-factor authentication for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to enable two-factor authentication.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task EnableTwoFactorAsync(User user)
    {
        user.TwoFactorEnabled = true;
        await this.userManager.UpdateAsync(user);
    }

    /// <summary>
    /// Disables two-factor authentication (TOTP) for the specified user.
    /// </summary>
    /// <param name="user">The user for whom TOTP should be disabled.</param>
    /// <returns>
    /// A <c>true</c> value if TOTP was successfully disabled; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method uses ASP.NET Identity's <see cref="UserManager{TUser}.SetTwoFactorEnabledAsync"/>
    /// to turn off 2FA, which internally updates the user in the database. Then it resets the
    /// authenticator key using <see cref="UserManager{TUser}.ResetAuthenticatorKeyAsync"/>, ensuring
    /// the user cannot use the previous TOTP codes.
    /// </remarks>
    public async Task<bool> DisableTotpAsync(User user)
    {
        var disableResult = await this.userManager.SetTwoFactorEnabledAsync(user, false);
        if (!disableResult.Succeeded)
        {
            return false;
        }

        await this.userManager.ResetAuthenticatorKeyAsync(user);

        return true;
    }

    /// <summary>
    /// Retrieves the TOTP backup (recovery) codes for the specified user.
    /// </summary>
    /// <param name="user">The user whose backup codes are requested.</param>
    /// <returns>A list of backup codes if the user is valid; otherwise, an empty list.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is null.</exception>
    public async Task<IReadOnlyList<string>> GetTotpBackupCodesAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "При отриманні резервних кодів TOTP користувач не може бути нульовим.");
        }

        // Generate 10 new two-factor recovery codes for the user
        var codes = await this.userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10)
                  ?? Array.Empty<string>();

        return codes.ToList();
    }

    /// <summary>
    /// Regenerates new TOTP (two-factor authentication) backup codes for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to regenerate backup codes.</param>
    /// <returns>
    /// A read-only list of newly generated backup codes.
    /// If the <paramref name="user"/> is <c>null</c>, an <see cref="ArgumentNullException"/> is thrown.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is <c>null</c>.</exception>
    public async Task<IReadOnlyList<string>> RegenerateTotpBackupCodesAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var codes = await this.userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        return codes?.ToList() ?? new List<string>();
    }

    /// <summary>
    /// Verifies a TOTP backup code for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the backup code should be verified.</param>
    /// <param name="code">The backup code provided by the user.</param>
    /// <returns>
    /// <c>true</c> if the backup code is valid and successfully redeemed; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> is <c>null</c>.</exception>
    public async Task<bool> VerifyTotpBackupCodeAsync(User user, string code)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            return false;
        }

        var isValid = await this.userManager.RedeemTwoFactorRecoveryCodeAsync(user, code);
        return isValid.Succeeded;
    }

    /// <summary>
    /// Confirms the user's phone number by setting <see cref="User.PhoneNumberConfirmed"/> to true.
    /// </summary>
    /// <param name="user">The user whose phone number is to be confirmed.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if updating the user in the database fails.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task ConfirmPhoneNumberAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        user.PhoneNumberConfirmed = true;

        var result = await this.userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            this.logger.LogWarning("Failed to confirm phone number for user {UserId}", user.Id);
            throw new InvalidOperationException("Не вдалося підтвердити номер телефону користувача.");
        }

        this.logger.LogInformation("Phone number confirmed for user {UserId}", user.Id);
    }

    /// <summary>
    /// Disables SMS-based 2FA for the specified user.
    /// Sets <see cref="User.PhoneNumberConfirmed"/> to false and updates the user in the database.
    /// </summary>
    /// <param name="user">The user for whom to disable SMS 2FA.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when updating the user in the database fails.</exception>
    public async Task DisableSms2FaAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        user.PhoneNumberConfirmed = false;

        var result = await this.userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            this.logger.LogWarning("Failed to disable SMS 2FA for user {UserId}", user.Id);
            throw new InvalidOperationException("Не вдалося відключити SMS 2FA користувача.");
        }

        this.logger.LogInformation("SMS 2FA disabled for user {UserId}", user.Id);
    }

    /// <summary>
    /// Retrieves the current two-factor authentication (2FA) status for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to get the 2FA status.</param>
    /// <returns>
    /// A <see cref="TwoFactorStatusResponseDto"/> containing the status of each 2FA method:
    /// <list type="bullet">
    /// <item><description><c>IsTwoFactorEnabled</c> — overall 2FA enabled flag.</description></item>
    /// <item><description><c>IsSms2FaEnabled</c> — SMS 2FA enabled flag based on <see cref="User.PhoneNumberConfirmed"/>.</description></item>
    /// </list>
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> is null.</exception>
    public TwoFactorStatusResponseDto GetTwoFactorStatus(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return new TwoFactorStatusResponseDto(
            IsTwoFactorEnabled: user.TwoFactorEnabled,
            IsSms2FaEnabled: user.PhoneNumberConfirmed);
    }

    /// <summary>
    /// Disables all two-factor authentication methods for the specified user.
    /// </summary>
    /// <param name="user">The user for whom all 2FA methods will be disabled.</param>
    /// <returns>True if all 2FA methods were successfully disabled; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="user"/> is null.</exception>
    public async Task<bool> DisableAllTwoFactorAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        user.TwoFactorEnabled = false;
        user.PhoneNumberConfirmed = false;

        var result = await this.userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            this.logger.LogWarning("Failed to disable all 2FA methods for user {UserId}", user.Id);
            return false;
        }

        this.logger.LogInformation("All 2FA methods disabled for user {UserId}", user.Id);
        return true;
    }

    /// <summary>
    /// Redeems a TOTP recovery code for the specified user.
    /// </summary>
    /// <param name="user">The user who is redeeming the recovery code.</param>
    /// <param name="code">The recovery code to redeem.</param>
    /// <returns>True if the recovery code was successfully redeemed; otherwise, false.</returns>
    public async Task<bool> RedeemRecoveryCodeAsync(User user, string code)
    {
        var result = await this.userManager.RedeemTwoFactorRecoveryCodeAsync(user, code);
        if (result.Succeeded)
        {
            this.logger.LogInformation("Recovery code redeemed successfully for user {UserId}", user.Id);
            return true;
        }

        this.logger.LogWarning("Failed recovery code attempt for user {UserId}", user.Id);
        return false;
    }
}

namespace PetCare.Infrastructure.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OtpNet;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.Events;
using PetCare.Domain.ValueObjects;
using PetCare.Infrastructure.Persistence;

/// <summary>
/// Service for user management operations using ASP.NET Core Identity.
/// </summary>
public sealed class UserService : IUserService
{
    private static readonly string[] ValidRoles = new[]
   {
        "User",
        "Admin",
        "ShelterManager",
        "Veterinarian",
        "Volunteer",
   };

    private readonly UserManager<User> userManager;
    private readonly AppDbContext dbContext;
    private readonly ILogger<UserService> logger;
    private readonly IZipcodebaseService zipcodebaseService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMemoryCache memoryCache;
    private readonly IUserRepository userRepository;
    private readonly IStorageService storageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class with the specified dependencies required for user.
    /// management, data access, logging, HTTP context, caching, user repository operations, and file storage.
    /// </summary>
    /// <param name="userManager">The user manager used to perform identity-related operations such as user creation, authentication, and role
    /// management.</param>
    /// <param name="dbContext">The application database context used for accessing and managing persistent data.</param>
    /// <param name="zipcodebaseService">The service used to perform zipcode-based operations, such as location lookups or distance calculations.</param>
    /// <param name="logger">The logger used to record diagnostic and operational information for the UserService.</param>
    /// <param name="httpContextAccessor">The accessor for the current HTTP context, enabling access to request-specific information.</param>
    /// <param name="memoryCache">The memory cache used for storing and retrieving frequently accessed data to improve performance.</param>
    /// <param name="userRepository">The user repository used for custom user data operations beyond standard identity management.</param>
    /// <param name="storageService">The file storage service used to manage user-related files and documents.</param>
    /// <exception cref="ArgumentNullException">Thrown if any of the parameters are null.</exception>
    public UserService(
        UserManager<User> userManager,
        AppDbContext dbContext,
        IZipcodebaseService zipcodebaseService,
        ILogger<UserService> logger,
        IHttpContextAccessor httpContextAccessor,
        IMemoryCache memoryCache,
        IUserRepository userRepository,
        IStorageService storageService)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        this.zipcodebaseService = zipcodebaseService ?? throw new ArgumentNullException(nameof(zipcodebaseService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
    }

    /// <summary>
    /// Creates a new user with the specified information and adds a domain event.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="firstName">The user's first name.</param>
    /// <param name="lastName">The user's last name.</param>
    /// <param name="phone">The user's phone number.</param>
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
     string phone,
     string? postalCode)
    {
        this.logger.LogInformation("Creating user with email: {Email}", email);

        // Створюємо агрегат User
        var user = User.Create(
            email: email,
            firstName: firstName,
            lastName: lastName,
            phone: phone,
            role: UserRole.User,
            postalCode: postalCode);

        // Якщо заданий postalCode, отримуємо адресу через ZipcodebaseService
        if (!string.IsNullOrWhiteSpace(postalCode))
        {
            try
            {
                // Передаємо CancellationToken для можливості скасування
                var address = await this.zipcodebaseService.ResolveAddressAsync(postalCode, default);
                if (address != null)
                {
                    user.UpdateAddress(address);
                    this.logger.LogInformation(
                        "Address resolved for postal code {PostalCode}: {Address}",
                        postalCode,
                        address.Value);
                }
                else
                {
                    user.UpdateAddress(Address.Unknown());
                    this.logger.LogWarning(
                        "Could not resolve address for postal code {PostalCode}. Default address set: {DefaultAddress}",
                        postalCode,
                        Address.Unknown().Value);
                }
            }
            catch (Exception ex)
            {
                user.UpdateAddress(Address.Unknown());
                this.logger.LogWarning(
                    ex,
                    "Не вдалося згенерувати адресу для postal code {PostalCode}. Default address set: {DefaultAddress}",
                    postalCode,
                    Address.Unknown().Value);
            }
        }

        this.logger.LogInformation("User ID before CreateAsync: {UserId}", user.Id);

        var result = await this.userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            this.logger.LogError("Failed to create user {Email}: {Errors}", email, errors);
            throw new InvalidOperationException($"Не вдалося створити користувача: {errors}");
        }

        this.logger.LogInformation("User ID after CreateAsync: {UserId}", user.Id);

        // Додаємо доменну подію
        user.AddUserCreatedEvent();
        this.logger.LogInformation("UserCreatedEvent added to user {UserId}", user.Id);

        // Збереження змін, щоб подія була опублікована
        await this.dbContext.SaveChangesAsync();

        user.ClearDomainEvents();

        this.logger.LogInformation("User created successfully: {UserId}", user.Id);
        return user;
    }

    /// <summary>
    /// Creates a new user based on Facebook user information without a password.
    /// </summary>
    /// <param name="fbUser">The Facebook user information.</param>
    /// <returns>The created <see cref="User"/> entity.</returns>
    public async Task<User> CreateUserFromFacebookAsync(FacebookUserInfoDto fbUser)
    {
        this.logger.LogInformation("Створення користувача через Facebook: {Email}", fbUser.Email);

        var user = User.Create(
            email: fbUser.Email,
            firstName: fbUser.FirstName,
            lastName: fbUser.LastName,
            phone: null,
            role: UserRole.User,
            postalCode: null);

        if (!string.IsNullOrWhiteSpace(fbUser.ProfilePhotoUrl))
        {
            user.UpdateAvatar(fbUser.ProfilePhotoUrl);
            this.logger.LogInformation("Profile photo додано для користувача {Email}", fbUser.Email);
        }

        var result = await this.userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            this.logger.LogError("Не вдалося створити користувача через Facebook {Email}: {Errors}", fbUser.Email, errors);
            throw new InvalidOperationException($"Не вдалося створити користувача через Facebook: {errors}");
        }

        // Додаємо доменну подію
        user.AddUserCreatedEvent();
        await this.dbContext.SaveChangesAsync();
        user.ClearDomainEvents();

        this.logger.LogInformation("Користувач через Facebook створений успішно: {UserId}", user.Id);
        return user;
    }

    /// <summary>
    /// Creates a new user based on Google user information without a password.
    /// </summary>
    /// <param name="googleUser">The Google user information.</param>
    /// <returns>The created <see cref="User"/> entity.</returns>
    public async Task<User> CreateUserFromGoogleAsync(GoogleUserInfoDto googleUser)
    {
        this.logger.LogInformation("Створення користувача через Google: {Email}", googleUser.Email);

        var user = User.Create(
            email: googleUser.Email,
            firstName: googleUser.FirstName,
            lastName: googleUser.LastName,
            phone: null,
            role: UserRole.User,
            postalCode: null);

        if (!string.IsNullOrWhiteSpace(googleUser.ProfilePhotoUrl))
        {
            user.UpdateAvatar(googleUser.ProfilePhotoUrl);
            this.logger.LogInformation("Profile photo додано для користувача {Email}", googleUser.Email);
        }

        var result = await this.userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            this.logger.LogError("Не вдалося створити користувача через Google {Email}: {Errors}", googleUser.Email, errors);
            throw new InvalidOperationException($"Не вдалося створити користувача через Google: {errors}");
        }

        // Додаємо доменну подію
        user.AddUserCreatedEvent();
        await this.dbContext.SaveChangesAsync();
        user.ClearDomainEvents();

        this.logger.LogInformation("Користувач через Google створений успішно: {UserId}", user.Id);
        return user;
    }

    /// <summary>
    /// Asynchronously updates the specified user in the data store.
    /// </summary>
    /// <param name="user">The user entity containing updated information to be saved. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user entity.</returns>
    public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        var updatedUser = await this.userRepository.UpdateAsync(user, cancellationToken);
        return updatedUser;
    }

    /// <summary>
    /// Asynchronously deletes the user with the specified identifier from the data store.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if a user with the specified <paramref name="userId"/> does not exist.</exception>
    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await this.userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new KeyNotFoundException($"Користувача з ідентифікатором '{userId}' не знайдено.");

        await this.userRepository.DeleteAsync(user, cancellationToken);
    }

    /// <summary>
    /// Updates the <see cref="User.LastLogin"/> property to the specified date and time,
    /// adds a domain event, and persists the changes via <see cref="UserManager{User}.UpdateAsync"/>.
    /// </summary>
    /// <param name="user">The user whose last login timestamp will be updated.</param>
    /// <param name="lastLogin">The date and time of the last login.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">Thrown if updating the user in the database fails.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SetLastLoginAsync(User user, DateTime lastLogin)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        // Агрегат оновлює стан і додає доменну подію
        user.SetLastLogin(lastLogin);

        await this.dbContext.SaveChangesAsync();

        this.logger.LogInformation("Last login updated for user {UserId}", user.Id);
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
    /// Asynchronously searches for a user by their phone number.
    /// </summary>
    /// <param name="phone">The phone number to search for.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// <see cref="User"/> if one exists with the specified phone number; otherwise, <c>null</c>.
    /// </returns>
    public async Task<User?> FindByPhoneAsync(string phone)
    {
        return await this.userManager.Users
            .FirstOrDefaultAsync(u => u.Phone == phone);
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

    /// <summary>
    /// Replaces the current role of a user with a new role, removing old roles and updating the <see cref="User.Role"/> field.
    /// </summary>
    /// <param name="user">The user entity whose role is being changed.</param>
    /// <param name="newRole">The name of the new role to assign.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="user"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="newRole"/> is null, empty, or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the role is invalid or updating roles via Identity fails.</exception>
    public async Task ReplaceRoleAsync(User user, string newRole)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (string.IsNullOrWhiteSpace(newRole))
        {
            throw new ArgumentException("Роль не може бути порожньою.", nameof(newRole));
        }

        // Перевірка, чи роль валідна
        if (!ValidRoles.Contains(newRole))
        {
            throw new InvalidOperationException($"Невідома роль: {newRole}");
        }

        // Отримуємо поточні ролі користувача
        var currentRoles = await this.userManager.GetRolesAsync(user);

        // Видаляємо всі поточні ролі
        if (currentRoles.Any())
        {
            var removeResult = await this.userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Не вдалося видалити старі ролі користувачу: {errors}");
            }
        }

        // Додаємо нову роль через Identity
        var addResult = await this.userManager.AddToRoleAsync(user, newRole);
        if (!addResult.Succeeded)
        {
            var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Не вдалося додати роль користувачу: {errors}");
        }

        // Оновлюємо поле Role в агрегаті User
        if (!Enum.TryParse<UserRole>(newRole, out var enumRole))
        {
            throw new InvalidOperationException($"Не вдалося конвертувати роль '{newRole}' в enum UserRole.");
        }

        user.SetRole(enumRole);

        // Зберігаємо зміни в БД
        await this.userManager.UpdateAsync(user);
    }

    /// <summary>
    /// Changes the password of the specified <see cref="User"/> by generating a reset token
    /// and applying the new password using <see cref="UserManager{User}"/>.
    /// Throws an exception if the password change fails.
    /// </summary>
    /// <param name="userId">The Id of the user whose password will be changed.</param>
    /// <param name="newPassword">The new plain text password to set.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the password reset operation fails.</exception>
    public async Task ChangePasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken)
    {
        // Завантажуємо користувача через UserManager (новий контекст)
        var user = await this.userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            throw new KeyNotFoundException("Користувача не знайдено.");
        }

        var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
        var result = await this.userManager.ResetPasswordAsync(user, token, newPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors?.Any() == true
                ? string.Join(", ", result.Errors.Select(e => e.Description))
                : "Невідома помилка при зміні пароля.";

            throw new InvalidOperationException($"Не вдалося змінити пароль: {errors}");
        }
    }

    /// <summary>
    /// Stores a temporary 2FA token for the user in memory with a specified TTL.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="token">The generated 2FA token.</param>
    /// <param name="ttl">The time-to-live for the token in memory.</param>
    /// <returns>A completed task.</returns>
    public Task Save2FaTokenAsync(Guid userId, string token, TimeSpan ttl)
    {
        // Ключ можна робити за UserId
        var cacheKey = $"2fa_token:{userId}";
        this.memoryCache.Set(cacheKey, token, ttl);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Verifies the provided 2FA token for the user.
    /// If the token matches the stored one, it is removed after successful validation.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="token">The token to verify.</param>
    /// <returns>
    /// <c>true</c> if the token is valid and removed after verification;
    /// <c>false</c> if the token is missing or does not match.
    /// </returns>
    public Task<bool> Verify2FaTokenAsync(Guid userId, string token)
    {
        var cacheKey = $"2fa_token:{userId}";
        if (this.memoryCache.TryGetValue<string>(cacheKey, out var savedToken))
        {
            if (savedToken == token)
            {
                // Токен пройшов валідацію → видаляємо його
                this.memoryCache.Remove(cacheKey);
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }

    /// <summary>
    /// Finds a user by their temporary 2FA token stored in memory.
    /// </summary>
    /// <param name="twoFaToken">The temporary 2FA token.</param>
    /// <returns>The <see cref="User"/> if found; otherwise, null.</returns>
    public async Task<User?> GetUserByTwoFaTokenAsync(string twoFaToken)
    {
        if (string.IsNullOrWhiteSpace(twoFaToken))
        {
            return null;
        }

        var allUsers = await this.userManager.Users.ToListAsync();

        foreach (var user in allUsers)
        {
            var cacheKey = $"2fa_token:{user.Id}";
            if (this.memoryCache.TryGetValue<string>(cacheKey, out var token))
            {
                if (token == twoFaToken)
                {
                    return user;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Asynchronously retrieves all adoption applications submitted by the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose adoption applications are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of <see cref="AdoptionApplication"/> objects representing the user's adoption applications. The
    /// list will be empty if the user has not submitted any applications.</returns>
    public async Task<IReadOnlyList<AdoptionApplication>> GetUserAdoptionApplicationsAsync(Guid userId, CancellationToken cancellationToken = default)
        => await this.userRepository.GetUserAdoptionApplicationsAsync(userId, cancellationToken);

    /// <summary>
    /// Asynchronously retrieves the list of events associated with the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose events are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of <see cref="Event"/> objects representing the user's events. The list will be empty if the
    /// user has no events.</returns>
    public async Task<IReadOnlyList<Event>> GetUserEventsAsync(Guid userId, CancellationToken cancellationToken = default)
        => await this.userRepository.GetUserEventsAsync(userId, cancellationToken);

    /// <summary>
    /// Asynchronously retrieves a paginated list of users, optionally filtered by search term and role.
    /// </summary>
    /// <param name="page">The zero-based page index indicating which page of results to retrieve. Must be greater than or equal to 0.</param>
    /// <param name="pageSize">The maximum number of users to include in the returned page. Must be greater than 0.</param>
    /// <param name="search">An optional search term used to filter users by name or other criteria. If null or empty, no search filtering is
    /// applied.</param>
    /// <param name="role">An optional role name used to filter users by their assigned role. If null or empty, users of all roles are
    /// included.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A tuple containing a read-only list of users for the specified page and the total count of users matching the
    /// filter criteria.</returns>
    public async Task<(IReadOnlyList<User> Users, int TotalCount)> GetUsersAsync(int page, int pageSize, string? search, string? role, CancellationToken cancellationToken = default)
        => await this.userRepository.GetUsersAsync(page, pageSize, search, role, cancellationToken);

    /// <summary>
    /// Asynchronously retrieves the list of shelter subscriptions associated with the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose shelter subscriptions are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of <see cref="ShelterSubscription"/> objects representing the user's shelter subscriptions.
    /// Returns an empty list if the user has no subscriptions.</returns>
    public async Task<IReadOnlyList<ShelterSubscription>> GetUserShelterSubscriptionsAsync(Guid userId, CancellationToken cancellationToken = default)
       => await this.userRepository.GetUserShelterSubscriptionsAsync(userId, cancellationToken);

    /// <summary>
    /// Asynchronously retrieves the list of animal subscriptions associated with the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose animal subscriptions are to be retrieved.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A read-only list of <see cref="AnimalSubscription"/> objects representing the user's animal subscriptions. The
    /// list will be empty if the user has no subscriptions.</returns>
    public async Task<IReadOnlyList<AnimalSubscription>> GetUserAnimalSubscriptionsAsync(Guid userId, CancellationToken cancellationToken = default)
        => await this.userRepository.GetUserAnimalSubscriptionsAsync(userId, cancellationToken);
}

namespace PetCare.Infrastructure.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    /// /// <param name="domainEventDispatcher">
    /// The domain event dispatcher responsible for publishing domain events after entity changes.
    /// </param>
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
    /// /// <param name="postalCode">
    /// Optional postal code (ZIP) of the user's address. Can be <c>null</c> if not provided.
    /// </param>
    /// <returns>The created <see cref="User"/> entity.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the user creation fails with validation errors.
    /// </exception>
    public async Task<User> CreateUserAsync(string email, string password, string firstName, string lastName, string phoneNumber, string? postalCode)
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

    public async Task<User?> GetCurrentUserAsync()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return null;

        var claimsPrincipal = httpContext.User;
        if (claimsPrincipal.Identity == null || !claimsPrincipal.Identity.IsAuthenticated)
            return null;

        var userIdStr = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? claimsPrincipal.FindFirstValue("id");

        if (!Guid.TryParse(userIdStr, out var userId))
            return null;

        return await FindByIdAsync(userId);
    }

    public async Task<string> GetEmailAsync(User user)
    {
        return await userManager.GetEmailAsync(user) ?? string.Empty;
    }

    public async Task<string?> GetAuthenticatorKeyAsync(User user)
    {
        return await userManager.GetAuthenticatorKeyAsync(user);
    }

    public async Task<string> ResetAuthenticatorKeyAsync(User user)
    {
        await userManager.ResetAuthenticatorKeyAsync(user);
        var key = await userManager.GetAuthenticatorKeyAsync(user);
        return key ?? throw new InvalidOperationException("Не вдалося згенерувати ключ TOTP.");
    }

    public async Task<string[]> GenerateNewTwoFactorRecoveryCodesAsync(User user, int count)
    {
        var codes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, count);
        return codes?.ToArray() ?? Array.Empty<string>();
    }
}

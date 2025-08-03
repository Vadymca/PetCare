// <copyright file="User.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>
namespace PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Represents a user in the system.
/// </summary>
public sealed class User : BaseEntity
{
    private User()
    {
        this.Email = Email.Create("default@example.com");
        this.PasswordHash = string.Empty;
        this.FirstName = string.Empty;
        this.LastName = string.Empty;
        this.Phone = PhoneNumber.Create("+000000000000");
        this.Preferences = new Dictionary<string, string>();
        this.Language = "uk";
    }

    private User(
        Email email,
        string passwordHash,
        string firstName,
        string lastName,
        PhoneNumber phone,
        UserRole role,
        Dictionary<string, string> preferences,
        int points,
        DateTime?
        lastLogin,
        string? profilePhoto,
        string language)
    {
        this.Email = email;
        this.PasswordHash = passwordHash;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Phone = phone;
        this.Role = role;
        this.Preferences = preferences ?? new();
        this.Points = points;
        this.LastLogin = lastLogin;
        this.ProfilePhoto = profilePhoto;
        this.Language = language;
        this.CreatedAt = DateTime.UtcNow;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    public Email Email { get; private set; }

    /// <summary>
    /// Gets the hashed password of the user.
    /// </summary>
    public string PasswordHash { get; private set; }

    /// <summary>
    /// Gets the first name of the user.
    /// </summary>
    public string FirstName { get; private set; }

    /// <summary>
    /// Gets the last name of the user.
    /// </summary>
    public string LastName { get; private set; }

    /// <summary>
    /// Gets the phone number of the user.
    /// </summary>
    public PhoneNumber Phone { get; private set; }

    /// <summary>
    /// Gets the role of the user.
    /// </summary>
    public UserRole Role { get; private set; }

    /// <summary>
    /// Gets the preferences of the user.
    /// </summary>
    public Dictionary<string, string> Preferences { get; private set; }

    /// <summary>
    /// Gets the points accumulated by the user.
    /// </summary>
    public int Points { get; private set; }

    /// <summary>
    /// Gets the date and time of the user's last login, if any. Can be null.
    /// </summary>
    public DateTime? LastLogin { get; private set; }

    /// <summary>
    /// Gets the URL of the user's profile photo, if any. Can be null.
    /// </summary>
    public string? ProfilePhoto { get; private set; }

    /// <summary>
    /// Gets the preferred language of the user.
    /// </summary>
    public string Language { get; private set; }

    /// <summary>
    /// Gets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the user was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Creates a new <see cref="User"/> instance with the specified parameters.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="passwordHash">The hashed password of the user.</param>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="phone">The phone number of the user.</param>
    /// <param name="role">The role of the user.</param>
    /// <param name="preferences">The preferences of the user, if any. Can be null.</param>
    /// <param name="points">The points accumulated by the user. Defaults to 0.</param>
    /// <param name="lastLogin">The date and time of the user's last login, if any. Can be null.</param>
    /// <param name="profilePhoto">The URL of the user's profile photo, if any. Can be null.</param>
    /// <param name="language">The preferred language of the user. Defaults to "uk".</param>
    /// <returns>A new instance of <see cref="User"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="passwordHash"/>, <paramref name="firstName"/>, <paramref name="lastName"/>, or <paramref name="language"/> is null, whitespace, or exceeds length limits, or when <paramref name="points"/> is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="email"/> or <paramref name="phone"/> is invalid according to their respective <see cref="ValueObject"/> creation methods.</exception>
    public static User Create(
        string email,
        string passwordHash,
        string firstName,
        string lastName,
        string phone,
        UserRole role,
        Dictionary<string, string>? preferences = null,
        int points = 0,
        DateTime? lastLogin = null,
        string? profilePhoto = null,
        string language = "uk")
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Хеш-пароль не може бути порожнім", nameof(passwordHash));
        }

        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > 50)
        {
            throw new ArgumentException("Ім'я невірне", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > 50)
        {
            throw new ArgumentException("Прізвище невірне", nameof(lastName));
        }

        if (string.IsNullOrWhiteSpace(language) || language.Length > 10)
        {
            throw new ArgumentException("Мова невірна", nameof(language));
        }

        if (points < 0)
        {
            throw new ArgumentException("Бали не можуть бути від'ємними", nameof(points));
        }

        return new User(
            Email.Create(email),
            passwordHash,
            firstName,
            lastName,
            PhoneNumber.Create(phone),
            role,
            preferences ?? new Dictionary<string, string>(),
            points,
            lastLogin,
            profilePhoto,
            language);
    }

    /// <summary>
    /// Updates the user's profile with the provided values.
    /// </summary>
    /// <param name="firstName">The new first name of the user, if provided. If null or whitespace, the first name remains unchanged.</param>
    /// <param name="lastName">The new last name of the user, if provided. If null or whitespace, the last name remains unchanged.</param>
    /// <param name="phone">The new phone number of the user, if provided. If null or whitespace, the phone number remains unchanged.</param>
    /// <param name="profilePhoto">The new URL of the user's profile photo, if provided. If null or whitespace, the profile photo remains unchanged.</param>
    /// <param name="language">The new preferred language of the user, if provided. If null or whitespace, the language remains unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="phone"/> is invalid according to the <see cref="PhoneNumber.Create"/> method.</exception>
    public void UpdateProfile(
        string? firstName = null,
        string? lastName = null,
        string? phone = null,
        string? profilePhoto = null,
        string? language = null)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            this.FirstName = firstName;
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            this.LastName = lastName;
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            this.Phone = PhoneNumber.Create(phone);
        }

        if (!string.IsNullOrWhiteSpace(profilePhoto))
        {
            this.ProfilePhoto = profilePhoto;
        }

        if (!string.IsNullOrWhiteSpace(language))
        {
            this.Language = language;
        }

        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds the specified amount of points to the user's total points.
    /// </summary>
    /// <param name="amount">The number of points to add. If negative, no points are added.</param>
    public void AddPoints(int amount)
    {
        if (amount < 0)
        {
            return;
        }

        this.Points += amount;
        this.UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the date and time of the user's last login.
    /// </summary>
    /// <param name="date">The date and time of the last login.</param>
    public void SetLastLogin(DateTime date)
    {
        this.LastLogin = date;
        this.UpdatedAt = DateTime.UtcNow;
    }
}

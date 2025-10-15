namespace PetCare.Application.Dtos.AuthDtos;

using System;

/// <summary>
/// Represents a data transfer object containing user account information, including identity, contact details, role,
/// preferences, and activity metadata.
/// </summary>
/// <param name="Id">The unique identifier for the user.</param>
/// <param name="Email">The email address associated with the user account. Cannot be null or empty.</param>
/// <param name="FirstName">The user's first name. Cannot be null or empty.</param>
/// <param name="LastName">The user's last name. Cannot be null or empty.</param>
/// <param name="Phone">The user's phone number. Cannot be null or empty.</param>
/// <param name="Role">The role assigned to the user, such as 'Admin' or 'User'. Cannot be null or empty.</param>
/// <param name="PostalCode">The user's postal code, or null if not provided.</param>
/// <param name="Address">The user's address, or null if not provided.</param>
/// <param name="Language">The user's preferred language code (for example, 'en' or 'fr'). Cannot be null or empty.</param>
/// <param name="CreatedAt">The date and time when the user account was created, in UTC.</param>
/// <param name="UpdatedAt">The date and time when the user account was last updated, in UTC.</param>
/// <param name="Points">The number of points currently associated with the user. Must be zero or greater.</param>
/// <param name="ProfilePhoto">The URL or identifier of the user's profile photo, or null if not set.</param>
/// <param name="LastLogin">The date and time of the user's most recent login, or null if the user has never logged in.</param>
/// <param name="Preferences">A read-only dictionary of user-specific preferences, or null if no preferences are set.</param>
public sealed record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string Phone,
    string Role,
    string? PostalCode,
    string? Address,
    string Language,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    int Points,
    string? ProfilePhoto,
    DateTime? LastLogin,
    IReadOnlyDictionary<string, string>? Preferences);

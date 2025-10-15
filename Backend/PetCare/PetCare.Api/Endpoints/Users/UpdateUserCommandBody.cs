namespace PetCare.Api.Endpoints.Users;

/// <summary>
/// Represents the data required to update an existing user's profile information.
/// </summary>
/// <param name="Email">The new email address for the user. Specify <see langword="null"/> to leave the email unchanged.</param>
/// <param name="Password">The new password for the user. Specify <see langword="null"/> to leave the password unchanged.</param>
/// <param name="FirstName">The user's updated first name. Specify <see langword="null"/> to leave the first name unchanged.</param>
/// <param name="LastName">The user's updated last name. Specify <see langword="null"/> to leave the last name unchanged.</param>
/// <param name="Phone">The user's updated phone number. Specify <see langword="null"/> to leave the phone number unchanged.</param>
/// <param name="Preferences">A dictionary of user preferences to update. Specify <see langword="null"/> to leave preferences unchanged.</param>
/// <param name="Points">The updated points value for the user. Specify <see langword="null"/> to leave the points unchanged.</param>
/// <param name="ProfilePhoto">The URL or identifier of the user's new profile photo. Specify <see langword="null"/> to leave the profile photo
/// unchanged.</param>
/// <param name="Language">The user's preferred language code. Specify <see langword="null"/> to leave the language unchanged.</param>
/// <param name="PostalCode">The user's updated postal code. Specify <see langword="null"/> to leave the postal code unchanged.</param>
public sealed record UpdateUserCommandBody(
    string? Email,
    string? Password,
    string? FirstName,
    string? LastName,
    string? Phone,
    Dictionary<string, string>? Preferences,
    int? Points,
    string? ProfilePhoto,
    string? Language,
    string? PostalCode);

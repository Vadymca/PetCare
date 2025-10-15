namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents user profile information obtained from a Google account, including email address, name, and profile photo
/// URL.
/// </summary>
/// <param name="Email">The email address associated with the Google account. Cannot be null.</param>
/// <param name="FirstName">The user's given name as provided by Google. Cannot be null.</param>
/// <param name="LastName">The user's family name as provided by Google. Cannot be null.</param>
/// <param name="ProfilePhotoUrl">The URL of the user's profile photo, or null if no photo is available.</param>
public sealed record GoogleUserInfoDto(
string Email,
string FirstName,
string LastName,
string? ProfilePhotoUrl);

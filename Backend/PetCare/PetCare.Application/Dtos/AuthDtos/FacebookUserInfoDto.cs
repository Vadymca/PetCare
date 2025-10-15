namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents user profile information obtained from Facebook, including email, name, and profile photo URL.
/// </summary>
/// <param name="Email">The email address associated with the Facebook user. Cannot be null or empty.</param>
/// <param name="FirstName">The first name of the Facebook user. Cannot be null or empty.</param>
/// <param name="LastName">The last name of the Facebook user. Cannot be null or empty.</param>
/// <param name="ProfilePhotoUrl">The URL of the user's profile photo, or null if no photo is available.</param>
public sealed record FacebookUserInfoDto(
string Email,
string FirstName,
string LastName,
string? ProfilePhotoUrl);

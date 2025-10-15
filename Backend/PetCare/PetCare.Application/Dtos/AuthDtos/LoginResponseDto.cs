namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the response returned after a login attempt, including authentication status, user information, and
/// related metadata.
/// </summary>
/// <param name="Status">The status of the login attempt. Typically indicates success, failure, or additional authentication requirements.</param>
/// <param name="AccessToken">The access token issued upon successful authentication, or null if authentication was not successful.</param>
/// <param name="User">The user information associated with the login attempt, or null if not applicable.</param>
/// <param name="Method">The authentication method used for the login attempt, such as password or two-factor authentication, or null if not
/// specified.</param>
/// <param name="HiddenPhoneNumber">A masked version of the user's phone number, used for verification or display purposes, or null if not applicable.</param>
/// <param name="Message">An optional message providing additional information about the login result, or null if not provided.</param>
/// <param name="TwoFaToken">A token required to complete two-factor authentication, or null if two-factor authentication is not needed.</param>
public record LoginResponseDto(
    string Status,
    string? AccessToken = null,
    UserDto? User = null,
    string? Method = null,
    string? HiddenPhoneNumber = null,
    string? Message = null,
    string? TwoFaToken = null);

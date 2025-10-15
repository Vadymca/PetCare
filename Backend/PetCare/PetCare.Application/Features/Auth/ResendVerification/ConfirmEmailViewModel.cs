namespace PetCare.Application.Features.Auth.ResendVerification;

/// <summary>
/// Represents the data required to confirm a user's email address, including the user's name and the confirmation link.
/// </summary>
/// <param name="UserName">The display name of the user whose email address is being confirmed.</param>
/// <param name="ConfirmationUrl">The URL that the user should visit to confirm their email address.</param>
public record ConfirmEmailViewModel(string UserName, string ConfirmationUrl);

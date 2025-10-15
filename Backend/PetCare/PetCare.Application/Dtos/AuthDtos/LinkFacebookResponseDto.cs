namespace PetCare.Application.Dtos.AuthDtos;

/// <summary>
/// Represents the response returned after attempting to link a Facebook account.
/// </summary>
/// <param name="Status">The status of the link operation. Typically indicates success or failure.</param>
/// <param name="Message">A message providing additional details about the result of the link operation.</param>
public sealed record LinkFacebookResponseDto(
string Status,
string Message);

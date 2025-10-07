namespace PetCare.Application.Dtos.AuthDtos;

public sealed record GoogleUserInfoDto(
string Email,
string FirstName,
string LastName,
string? ProfilePhotoUrl);

namespace PetCare.Application.Dtos.AuthDtos;

public sealed record FacebookUserInfoDto(
string Email,
string FirstName,
string LastName,
string? ProfilePhotoUrl);

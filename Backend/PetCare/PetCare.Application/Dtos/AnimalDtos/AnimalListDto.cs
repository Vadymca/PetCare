namespace PetCare.Application.Dtos.AnimalDtos;

public sealed record AnimalListDto(
Guid Id,
string Slug,
string Name,
string? Photo,
string Status,
string? Birthday,
string Gender,
bool IsUnderCare,
SpecieDto? Specie,
ShelterInfoDto Shelter,
BreedDto? Breed);

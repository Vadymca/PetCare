namespace PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents a data transfer object containing detailed information about an animal, including identification, status,
/// and related entities.
/// </summary>
/// <param name="Id">The unique identifier of the animal.</param>
/// <param name="Slug">The URL-friendly slug used to identify the animal in web contexts. Cannot be null or empty.</param>
/// <param name="Name">The display name of the animal. Cannot be null or empty.</param>
/// <param name="Photo">The URL of the animal's photo, or null if no photo is available.</param>
/// <param name="Status">The current status of the animal (for example, 'Available', 'Adopted', or 'Fostered'). Cannot be null or empty.</param>
/// <param name="Birthday">The animal's date of birth as a string, or null if the birthday is unknown. The format may vary depending on the
/// data source.</param>
/// <param name="Gender">The gender of the animal (for example, 'Male' or 'Female'). Cannot be null or empty.</param>
/// <param name="IsUnderCare">true if the animal is currently under the care of the shelter; otherwise, false.</param>
/// <param name="Specie">The species information for the animal, or null if not specified.</param>
/// <param name="Shelter">The shelter where the animal is currently located. Cannot be null.</param>
/// <param name="Breed">The breed information for the animal, or null if not specified.</param>
public sealed record AnimalListDto(
Guid Id,
string Slug,
string Name,
string? Photo,
string Status,
string? Birthday,
string Gender,
bool IsUnderCare,
SpecieDto? Species,
ShelterInfoDto Shelter,
BreedDto? Breed);

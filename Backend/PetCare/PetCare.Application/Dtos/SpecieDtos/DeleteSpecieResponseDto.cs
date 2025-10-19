namespace PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Represents the response returned after deleting a species.
/// </summary>
/// <param name="Success">Indicates whether the deletion was successful.</param>
/// <param name="Message">A descriptive message about the operation result.</param>
public sealed record DeleteSpecieResponseDto(bool Success, string Message);

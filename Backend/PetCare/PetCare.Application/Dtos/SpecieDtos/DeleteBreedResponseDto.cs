namespace PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Response for a successful breed deletion.
/// </summary>
/// <param name="Success">Indicates if the deletion was successful.</param>
/// <param name="Message">Additional message.</param>
public sealed record DeleteBreedResponseDto(
    bool Success,
    string Message);

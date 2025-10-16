namespace PetCare.Application.Dtos.ShelterDtos;

/// <summary>
/// Response DTO for delete operations.
/// </summary>
/// <param name="Success">True if deletion was successful.</param>
/// <param name="Message">Optional message about the operation result.</param>
public sealed record DeleteShelterResponseDto(bool Success, string Message);

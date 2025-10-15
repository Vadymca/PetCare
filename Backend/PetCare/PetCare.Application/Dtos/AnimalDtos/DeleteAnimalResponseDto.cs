namespace PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents the result of an animal deletion operation, including whether the operation succeeded and an associated
/// message.
/// </summary>
/// <param name="Success">A value indicating whether the animal was successfully deleted. Set to <see langword="true"/> if the deletion was
/// successful; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message providing additional information about the result of the deletion operation. May contain error details or
/// confirmation text.</param>
public sealed record DeleteAnimalResponseDto(
bool Success,
string Message);

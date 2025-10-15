namespace PetCare.Application.Dtos.AnimalDtos;

/// <summary>
/// Represents the result of an unsubscribe operation, including a message describing the outcome.
/// </summary>
/// <param name="Message">A message that provides details about the result of the unsubscribe operation. This may include success confirmation
/// or an explanation of any issues encountered.</param>
public sealed record UnsubscribeResultDto(string Message);

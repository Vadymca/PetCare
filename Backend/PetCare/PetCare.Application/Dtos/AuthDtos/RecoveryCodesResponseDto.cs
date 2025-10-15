namespace PetCare.Application.Dtos.AuthDtos;

using System.Collections.Generic;

/// <summary>
/// Represents the result of a recovery code generation operation, including the outcome, a status message, and the
/// generated recovery codes.
/// </summary>
/// <param name="Success">true if the recovery codes were generated successfully; otherwise, false.</param>
/// <param name="Message">A message describing the result of the operation. Typically contains error details if the operation was not
/// successful.</param>
/// <param name="Codes">A read-only list of generated recovery codes. The list is empty if the operation was not successful.</param>
public sealed record RecoveryCodesResponseDto(
    bool Success,
    string Message,
    IReadOnlyList<string> Codes);
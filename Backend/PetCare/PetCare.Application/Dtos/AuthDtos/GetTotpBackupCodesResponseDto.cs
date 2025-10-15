namespace PetCare.Application.Dtos.AuthDtos;

using System.Collections.Generic;

/// <summary>
/// Represents the response returned after requesting TOTP backup codes, including the operation result, a message, and
/// the list of backup codes if successful.
/// </summary>
/// <param name="Success">A value indicating whether the request for TOTP backup codes was successful. Set to <see langword="true"/> if the
/// operation succeeded; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message providing additional information about the result of the request. This may include error details or
/// success confirmation.</param>
/// <param name="BackupCodes">A read-only list of backup codes generated for TOTP authentication, or <see langword="null"/> if the request was not
/// successful.</param>
public sealed record GetTotpBackupCodesResponseDto(
bool Success,
string Message,
IReadOnlyList<string>? BackupCodes);

namespace PetCare.Application.Dtos.AuthDtos;

using System.Collections.Generic;

public sealed record GetTotpBackupCodesResponseDto(
bool Success,
string Message,
IReadOnlyList<string>? BackupCodes);

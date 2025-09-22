namespace PetCare.Application.Dtos.AuthDtos;
using System.Collections.Generic;

public sealed record RecoveryCodesResponseDto(
    bool Success,
    string Message,
    IReadOnlyList<string> Codes);
namespace PetCare.Application.Dtos.AuthDtos;
using System.Collections.Generic;

public sealed record RecoveryCodesResponseDto(IReadOnlyList<string> Codes);

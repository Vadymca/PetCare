namespace PetCare.Application.Dtos.UserDtos;

using PetCare.Application.Dtos.AuthDtos;
using System.Collections.Generic;

public sealed record GetUsersResponseDto(
 IReadOnlyList<UserDto> Users,
 int TotalCount
);

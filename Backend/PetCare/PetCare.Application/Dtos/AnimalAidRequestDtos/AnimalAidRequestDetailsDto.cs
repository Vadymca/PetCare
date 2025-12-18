namespace PetCare.Application.Dtos.AnimalAidRequestDtos;

using System;
using System.Collections.Generic;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Domain.Enums;

/// <summary>
/// Data transfer object representing the full details of an animal aid request.
/// </summary>
public sealed record AnimalAidRequestDetailsDto(
Guid Id,
string Slug,
ShelterInfoDto? Shelter,
string Title,
string Description,
AidCategory Category,
decimal EstimatedCost,
decimal AllreadyDonated,
int DonationsCount,
AidStatus Status,
IReadOnlyList<string>? Photos,
string? CuratorFullName,
string? ContactPhone,
DateTime CreatedAt);

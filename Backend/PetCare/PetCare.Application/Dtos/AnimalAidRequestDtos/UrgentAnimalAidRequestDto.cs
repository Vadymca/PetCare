namespace PetCare.Application.Dtos.AnimalAidRequestDtos;

using System;
using System.Collections.Generic;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Represents detailed information about an urgent animal aid request.
/// </summary>
public sealed record UrgentAnimalAidRequestDto(
    Guid Id,
    string Title,
    decimal EstimatedCost,
    decimal CollectedAmount,
    int DonationsCount,
    IReadOnlyList<DonationListDto> Donations);

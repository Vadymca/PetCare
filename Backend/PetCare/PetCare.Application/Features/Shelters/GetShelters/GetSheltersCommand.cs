namespace PetCare.Application.Features.Shelters.GetShelters;

using MediatR;
using PetCare.Application.Dtos.ShelterDtos;

/// <summary>
/// Represents a request to retrieve a paginated list of shelters.
/// </summary>
/// <param name="Page">The page number (1-based).</param>
/// <param name="PageSize">The number of shelters per page.</param>
public sealed record GetSheltersCommand(int Page = 1, int PageSize = 20)
    : IRequest<GetSheltersResponseDto>;

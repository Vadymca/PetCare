namespace PetCare.Application.Features.Shelters.GetShelterBySlug;

using MediatR;
using PetCare.Application.Dtos.ShelterDtos;

/// <summary>
/// Represents a request to retrieve a shelter by its unique slug identifier.
/// </summary>
/// <param name="Slug">The unique slug that identifies the shelter to retrieve. Cannot be null or empty.</param>
public sealed record GetShelterBySlugCommand(
    string Slug)
    : IRequest<ShelterDto>;

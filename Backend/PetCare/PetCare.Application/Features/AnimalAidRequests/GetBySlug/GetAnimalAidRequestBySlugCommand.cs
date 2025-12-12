namespace PetCare.Application.Features.AnimalAidRequests.GetBySlug;

using MediatR;
using PetCare.Application.Dtos.AnimalAidRequestDtos;

/// <summary>
/// Represents a request to retrieve an animal aid request by its unique slug.
/// </summary>
/// <param name="Slug">The slug that uniquely identifies the aid request.</param>
public sealed record GetAnimalAidRequestBySlugCommand(string Slug) : IRequest<AnimalAidRequestDetailsDto?>;

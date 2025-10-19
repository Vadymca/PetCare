namespace PetCare.Application.Features.Breeds.GetAllBreeds;

using MediatR;
using PetCare.Application.Dtos.SpecieDtos;

/// <summary>
/// Represents a request to retrieve all available dog breeds.
/// </summary>
/// <remarks>Use this command with a mediator to obtain a list of all breeds. The response contains breed
/// details as defined by <see cref="GetAllBreedsResponseDto"/>.</remarks>
public sealed record GetAllBreedsCommand() : IRequest<GetAllBreedsResponseDto>;

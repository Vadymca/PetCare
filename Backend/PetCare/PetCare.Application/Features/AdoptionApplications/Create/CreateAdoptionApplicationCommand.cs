namespace PetCare.Application.Features.AdoptionApplications.Create;

using System;
using MediatR;
using PetCare.Application.Dtos.AdoptionApplicationDtos;

/// <summary>
/// Command to create a new adoption application.
/// </summary>
/// <param name="UserId">The identifier of the user submitting the application.</param>
/// <param name="AnimalId">The identifier of the animal to be adopted.</param>
/// <param name="Comment">Optional comment provided by the user.</param>
public sealed record CreateAdoptionApplicationCommand(
    Guid UserId,
    Guid AnimalId,
    string? Comment) : IRequest<AdoptionApplicationDetailsDto>;

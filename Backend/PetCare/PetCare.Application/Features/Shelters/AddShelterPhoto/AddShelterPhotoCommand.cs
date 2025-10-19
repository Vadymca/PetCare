namespace PetCare.Application.Features.Shelters.AddShelterPhoto;

using System;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;

/// <summary>
/// Represents a request to add a photo to a shelter.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter.</param>
/// <param name="PhotoUrl">The URL of the photo to add to the shelter.</param>
public sealed record AddShelterPhotoCommand(Guid ShelterId, string PhotoUrl) : IRequest<ShelterDto>;

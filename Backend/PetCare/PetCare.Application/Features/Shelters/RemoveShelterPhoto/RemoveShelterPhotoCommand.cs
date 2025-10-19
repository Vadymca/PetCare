namespace PetCare.Application.Features.Shelters.RemoveShelterPhoto;

using System;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;

/// <summary>
/// Represents a request to remove a photo from a shelter identified by its unique identifier.
/// </summary>
/// <param name="ShelterId">The unique identifier of the shelter from which the photo will be removed.</param>
/// <param name="PhotoUrl">The URL of the photo to be removed from the shelter.</param>
public sealed record RemoveShelterPhotoCommand(
Guid ShelterId,
string PhotoUrl)
: IRequest<ShelterDto>;

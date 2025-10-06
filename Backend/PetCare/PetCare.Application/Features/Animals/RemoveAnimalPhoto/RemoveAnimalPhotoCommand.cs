namespace PetCare.Application.Features.Animals.RemoveAnimalPhoto;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using System;

public record RemoveAnimalPhotoCommand(
    Guid AnimalId,
    string PhotoUrl)
    : IRequest<AnimalDto>;

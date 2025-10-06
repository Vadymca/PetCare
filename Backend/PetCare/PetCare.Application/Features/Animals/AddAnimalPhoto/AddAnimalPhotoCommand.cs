namespace PetCare.Application.Features.Animals.AddAnimalPhoto;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using System;

public record AddAnimalPhotoCommand(
    Guid AnimalId,
    string PhotoUrl)
    : IRequest<AnimalDto>;

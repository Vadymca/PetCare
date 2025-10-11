namespace PetCare.Application.Features.Animals.AddAnimalPhoto;

using System;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;

public record AddAnimalPhotoCommand(
    Guid AnimalId,
    string PhotoUrl)
    : IRequest<AnimalDto>;

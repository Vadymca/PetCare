namespace PetCare.Application.Features.Animals.DeleteAnimal;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using System;

public sealed record DeleteAnimalCommand(Guid Id) : IRequest<DeleteAnimalResponseDto>;

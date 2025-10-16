namespace PetCare.Application.Features.Animals.RemoveAnimalPhoto;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for removing a photo from an animal.
/// </summary>
public class RemoveAnimalPhotoHandler : IRequestHandler<RemoveAnimalPhotoCommand, AnimalDto>
{
    private readonly IAnimalService animalService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveAnimalPhotoHandler"/> class.
    /// </summary>
    /// <param name="animalService">The animal service.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public RemoveAnimalPhotoHandler(IAnimalService animalService, IMapper mapper)
    {
        this.animalService = animalService ?? throw new ArgumentNullException(nameof(animalService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<AnimalDto> Handle(RemoveAnimalPhotoCommand request, CancellationToken cancellationToken)
    {
        var removed = await this.animalService.RemovePhotoAsync(request.AnimalId, request.PhotoUrl, cancellationToken);

        if (!removed)
        {
            throw new InvalidOperationException($"Фото не знайдено для тварини з Id '{request.AnimalId}'.");
        }

        var updatedAnimal = await this.animalService.GetByIdAsync(request.AnimalId, cancellationToken);

        return this.mapper.Map<AnimalDto>(updatedAnimal!);
    }
}

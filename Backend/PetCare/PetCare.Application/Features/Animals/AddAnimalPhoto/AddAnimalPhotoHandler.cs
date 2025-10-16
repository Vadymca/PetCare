namespace PetCare.Application.Features.Animals.AddAnimalPhoto;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for adding a photo to an animal.
/// </summary>
public class AddAnimalPhotoHandler : IRequestHandler<AddAnimalPhotoCommand, AnimalDto>
{
    private readonly IAnimalService animalService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddAnimalPhotoHandler"/> class with the specified animal service and object.
    /// mapper.
    /// </summary>
    /// <param name="animalService">The service used to manage animal-related operations. Cannot be null.</param>
    /// <param name="mapper">The object mapper used to map between domain and data transfer objects. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if animalService or mapper is null.</exception>
    public AddAnimalPhotoHandler(IAnimalService animalService, IMapper mapper)
    {
        this.animalService = animalService ?? throw new ArgumentNullException(nameof(animalService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<AnimalDto> Handle(AddAnimalPhotoCommand request, CancellationToken cancellationToken)
    {
        await this.animalService.AddPhotoAsync(request.AnimalId, request.PhotoUrl, cancellationToken);

        var updatedAnimal = await this.animalService.GetByIdAsync(request.AnimalId, cancellationToken);

        return this.mapper.Map<AnimalDto>(updatedAnimal!);
    }
}

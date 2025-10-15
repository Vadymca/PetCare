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
    private readonly IAnimalRepository repository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddAnimalPhotoHandler"/> class.
    /// </summary>
    /// <param name="repository">The animal repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public AddAnimalPhotoHandler(IAnimalRepository repository, IMapper mapper)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<AnimalDto> Handle(AddAnimalPhotoCommand request, CancellationToken cancellationToken)
    {
        await this.repository.AddPhotoAsync(request.AnimalId, request.PhotoUrl, cancellationToken);

        var updatedAnimal = await this.repository.GetByIdAsync(request.AnimalId, cancellationToken);

        return this.mapper.Map<AnimalDto>(updatedAnimal!);
    }
}

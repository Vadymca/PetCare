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
    private readonly IAnimalRepository repository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveAnimalPhotoHandler"/> class.
    /// </summary>
    /// <param name="repository">The animal repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public RemoveAnimalPhotoHandler(IAnimalRepository repository, IMapper mapper)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<AnimalDto> Handle(RemoveAnimalPhotoCommand request, CancellationToken cancellationToken)
    {
        var removed = await this.repository.RemovePhotoAsync(request.AnimalId, request.PhotoUrl, cancellationToken);

        if (!removed)
        {
            throw new InvalidOperationException($"Фото не знайдено для тварини з Id '{request.AnimalId}'.");
        }

        var updatedAnimal = await this.repository.GetByIdAsync(request.AnimalId, cancellationToken);

        return this.mapper.Map<AnimalDto>(updatedAnimal!);
    }
}

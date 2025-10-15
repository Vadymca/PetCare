namespace PetCare.Application.Features.Animals.GetAnimalBySlug;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for processing <see cref="GetAnimalBySlugCommand"/>.
/// </summary>
public sealed class GetAnimalBySlugCommandHandler : IRequestHandler<GetAnimalBySlugCommand, AnimalDto>
{
    private readonly IAnimalRepository animalRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAnimalBySlugCommandHandler"/> class.
    /// </summary>
    /// <param name="mapper">The mapper instance.</param>
    /// <param name="animalRepository">The animal repository.</param>
    public GetAnimalBySlugCommandHandler(IAnimalRepository animalRepository, IMapper mapper)
    {
        this.animalRepository = animalRepository;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<AnimalDto> Handle(GetAnimalBySlugCommand request, CancellationToken cancellationToken)
    {
        var animal = await this.animalRepository.GetBySlugAsync(request.Slug, cancellationToken)
                     ?? throw new InvalidOperationException($"Тварину зі slug '{request.Slug}' не знайдено.");

        return this.mapper.Map<AnimalDto>(animal);
    }
}

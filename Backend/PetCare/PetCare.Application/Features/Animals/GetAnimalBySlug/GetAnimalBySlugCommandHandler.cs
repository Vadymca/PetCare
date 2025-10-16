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
    private readonly IAnimalService animalService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAnimalBySlugCommandHandler"/> class.
    /// </summary>
    /// <param name="mapper">The mapper instance.</param>
    /// <param name="animalService">The animal service instance.</param>
    public GetAnimalBySlugCommandHandler(IAnimalService animalService, IMapper mapper)
    {
        this.animalService = animalService ?? throw new ArgumentNullException(nameof(animalService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public async Task<AnimalDto> Handle(GetAnimalBySlugCommand request, CancellationToken cancellationToken)
    {
        var animal = await this.animalService.GetBySlugAsync(request.Slug, cancellationToken)
                     ?? throw new InvalidOperationException($"Тварину зі slug '{request.Slug}' не знайдено.");

        return this.mapper.Map<AnimalDto>(animal);
    }
}

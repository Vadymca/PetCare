namespace PetCare.Application.Features.Animals.GetAnimalById;

using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for processing <see cref="GetAnimalByIdCommand"/>.
/// </summary>
public sealed class GetAnimalByIdCommandHandler : IRequestHandler<GetAnimalByIdCommand, AnimalDto?>
{
    private readonly IAnimalService animalService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAnimalByIdCommandHandler"/> class.
    /// </summary>
    /// <param name="animalService">The animal service instance.</param>
    /// <param name="mapper">The mapper instance.</param>
    public GetAnimalByIdCommandHandler(IAnimalService animalService, IMapper mapper)
    {
        this.animalService = animalService ?? throw new ArgumentNullException(nameof(animalService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public async Task<AnimalDto?> Handle(GetAnimalByIdCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Id не може бути порожнім.", nameof(request.Id));
        }

        var animal = await this.animalService.GetByIdAsync(request.Id, cancellationToken)
     ?? throw new InvalidOperationException("Тварину не знайдено.");

        return this.mapper.Map<AnimalDto>(animal);
    }
}

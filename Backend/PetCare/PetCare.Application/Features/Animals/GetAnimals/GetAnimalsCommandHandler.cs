namespace PetCare.Application.Features.Animals.GetAnimals;

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="GetAnimalsCommand"/>.
/// </summary>
public sealed class GetAnimalsCommandHandler
    : IRequestHandler<GetAnimalsCommand, GetAnimalsResponseDto>
{
    private readonly IAnimalService animalService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAnimalsCommandHandler"/> class.
    /// Handles the retrieval of animal data by processing the associated command.
    /// </summary>
    /// <remarks>This class is responsible for coordinating the retrieval of animal data from the repository
    /// and mapping it to the appropriate output format. Ensure that both <paramref name="repository"/> and <paramref
    /// name="mapper"/> are properly initialized before using this handler.</remarks>
    /// <param name="animalService">The service used to interact with animal data.</param>
    /// <param name="mapper">The mapper used to transform data between domain models and DTOs.</param>
    public GetAnimalsCommandHandler(IAnimalService animalService, IMapper mapper)
    {
        this.animalService = animalService ?? throw new ArgumentNullException(nameof(animalService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<GetAnimalsResponseDto> Handle(
        GetAnimalsCommand request,
        CancellationToken cancellationToken)
    {
        var (animals, total) = await this.animalService.GetAnimalsAsync(
             request.Page,
             request.PageSize,
             request.Sizes,
             request.Genders,
             request.MinAge,
             request.MaxAge,
             request.CareCosts,
             request.IsSterilized,
             request.IsUndercare,
             request.ShelterId,
             request.Statuses,
             request.SpecieId,
             request.BreedId,
             request.Search,
             request.AnimalTypeFilter,
             cancellationToken);

        var animalDtos = this.mapper.Map<IReadOnlyList<AnimalListDto>>(animals);

        return new GetAnimalsResponseDto(animalDtos, total);
    }
}

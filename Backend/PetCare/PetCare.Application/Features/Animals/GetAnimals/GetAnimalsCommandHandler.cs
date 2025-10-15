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
    private readonly IAnimalRepository repository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAnimalsCommandHandler"/> class.
    /// Handles the retrieval of animal data by processing the associated command.
    /// </summary>
    /// <remarks>This class is responsible for coordinating the retrieval of animal data from the repository
    /// and mapping it to the appropriate output format. Ensure that both <paramref name="repository"/> and <paramref
    /// name="mapper"/> are properly initialized before using this handler.</remarks>
    /// <param name="repository">The repository used to access animal data.</param>
    /// <param name="mapper">The mapper used to transform data between domain models and DTOs.</param>
    public GetAnimalsCommandHandler(IAnimalRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<GetAnimalsResponseDto> Handle(
        GetAnimalsCommand request,
        CancellationToken cancellationToken)
    {
        var (animals, total) = await this.repository.GetAnimalsAsync(
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
             cancellationToken);

        var animalDtos = this.mapper.Map<IReadOnlyList<AnimalListDto>>(animals);

        return new GetAnimalsResponseDto(animalDtos, total);
    }
}

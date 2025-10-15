namespace PetCare.Application.Features.Animals.CreateAnimal;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Handler for processing <see cref="CreateAnimalCommand"/>.
/// </summary>
public sealed class CreateAnimalCommandHandler : IRequestHandler<CreateAnimalCommand, AnimalDto>
{
    private readonly IAnimalRepository animalRepository;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAnimalCommandHandler"/> class.
    /// </summary>
    /// <param name="animalRepository">The animal repository.</param>
    /// <param name="mapper">The mapper instance.</param>
    public CreateAnimalCommandHandler(
        IAnimalRepository animalRepository,
        IMapper mapper)
    {
        this.animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public async Task<AnimalDto> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
    {
        Birthday? birthdayVo = request.Birthday.HasValue
            ? Birthday.Create(request.Birthday.Value)
            : null;

        var addedAnimal = await this.animalRepository.CreateAsync(
             userId: request.UserId,
             name: request.Name,
             breedId: request.BreedId,
             birthday: birthdayVo,
             gender: request.Gender,
             description: request.Description,
             healthConditions: request.HealthConditions,
             specialNeeds: request.SpecialNeeds,
             temperaments: request.Temperaments,
             size: request.Size,
             photos: request.Photos,
             videos: request.Videos,
             shelterId: request.ShelterId,
             status: request.Status,
             careCost: request.CareCost,
             adoptionRequirements: request.AdoptionRequirements,
             microchipId: request.MicrochipId,
             weight: request.Weight,
             height: request.Height,
             color: request.Color,
             isSterilized: request.IsSterilized,
             isUnderCare: request.IsUnderCare,
             haveDocuments: request.HaveDocuments,
             cancellationToken: cancellationToken);

        return this.mapper.Map<AnimalDto>(addedAnimal);
    }
}

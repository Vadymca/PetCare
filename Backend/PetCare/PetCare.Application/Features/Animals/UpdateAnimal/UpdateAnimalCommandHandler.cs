namespace PetCare.Application.Features.Animals.UpdateAnimal;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Handler for processing <see cref="UpdateAnimalCommand"/>.
/// </summary>
public sealed class UpdateAnimalCommandHandler : IRequestHandler<UpdateAnimalCommand, AnimalDto>
{
    private readonly IAnimalService animalService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAnimalCommandHandler"/> class.
    /// </summary>
    /// <param name="animalService">The animal service instance.</param>
    /// <param name="mapper">The mapper instance.</param>
    public UpdateAnimalCommandHandler(IAnimalService animalService, IMapper mapper)
    {
        this.animalService = animalService ?? throw new ArgumentNullException(nameof(animalService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public async Task<AnimalDto> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = await this.animalService.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Тварину з Id '{request.Id}' не знайдено.");

        // 🔹 Основні поля
        animal.Update(
            name: request.Name,
            birthday: request.Birthday is null ? null : Birthday.Create(request.Birthday.Value),
            gender: request.Gender,
            description: request.Description,
            status: request.Status,
            adoptionRequirements: request.AdoptionRequirements,
            microchipId: request.MicrochipId,
            weight: request.Weight,
            height: request.Height,
            color: request.Color,
            isSterilized: request.IsSterilized,
            haveDocuments: request.HaveDocuments);

        // 🔹 Колекції
        if (request.HealthConditions is not null)
        {
            animal.UpdateHealthConditions(request.HealthConditions);
        }

        if (request.SpecialNeeds is not null)
        {
            animal.UpdateSpecialNeeds(request.SpecialNeeds);
        }

        if (request.Temperaments is not null)
        {
            animal.UpdateTemperaments(request.Temperaments);
        }

        // 🔹 Size / Specie / CareCost
        if (request.Size.HasValue)
        {
            animal.UpdateSize(request.Size.Value);
        }

        if (request.CareCost.HasValue)
        {
            animal.UpdateCareCost(request.CareCost.Value);
        }

        await this.animalService.UpdateAsync(animal, cancellationToken);

        return this.mapper.Map<AnimalDto>(animal);
    }
}

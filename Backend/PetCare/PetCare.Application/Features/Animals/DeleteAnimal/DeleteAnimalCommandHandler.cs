namespace PetCare.Application.Features.Animals.DeleteAnimal;

using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;
using System;
using System.Threading.Tasks;

/// <summary>
/// Handler for processing <see cref="DeleteAnimalCommand"/>.
/// </summary>
public sealed class DeleteAnimalCommandHandler : IRequestHandler<DeleteAnimalCommand, DeleteAnimalResponseDto>
{
    private readonly IAnimalRepository animalRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteAnimalCommandHandler"/> class.
    /// </summary>
    /// <param name="animalRepository">The animal repository.</param>
    public DeleteAnimalCommandHandler(IAnimalRepository animalRepository)
    {
        this.animalRepository = animalRepository;
    }

    /// <inheritdoc />
    public async Task<DeleteAnimalResponseDto> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = await this.animalRepository.GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new InvalidOperationException($"Тварину з Id '{request.Id}' не знайдено.");

        await this.animalRepository.DeleteAsync(animal, cancellationToken);
        return new DeleteAnimalResponseDto(true, $"Тварина з Id '{request.Id}' успішно видалена.");
    }
}

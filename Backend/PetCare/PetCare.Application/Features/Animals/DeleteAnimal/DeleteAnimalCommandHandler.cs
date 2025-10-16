namespace PetCare.Application.Features.Animals.DeleteAnimal;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for processing <see cref="DeleteAnimalCommand"/>.
/// </summary>
public sealed class DeleteAnimalCommandHandler : IRequestHandler<DeleteAnimalCommand, DeleteAnimalResponseDto>
{
    private readonly IAnimalService animalService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteAnimalCommandHandler"/> class.
    /// </summary>
    /// <param name="animalService">The animal service.</param>
    public DeleteAnimalCommandHandler(IAnimalService animalService)
    {
        this.animalService = animalService ?? throw new ArgumentNullException(nameof(animalService));
    }

    /// <inheritdoc />
    public async Task<DeleteAnimalResponseDto> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
    {
        await this.animalService.DeleteAsync(request.Id, cancellationToken);
        return new DeleteAnimalResponseDto(true, $"Тварина з Id '{request.Id}' успішно видалена.");
    }
}

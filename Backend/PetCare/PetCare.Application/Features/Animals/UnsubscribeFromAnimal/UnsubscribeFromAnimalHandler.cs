namespace PetCare.Application.Features.Animals.UnsubscribeFromAnimal;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the unsubscription of a user from an animal.
/// </summary>
public class UnsubscribeFromAnimalHandler : IRequestHandler<UnsubscribeFromAnimalCommand, UnsubscribeResultDto>
{
    private readonly IAnimalService animalService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsubscribeFromAnimalHandler"/> class.
    /// </summary>
    /// <param name="animalService">The animal service.</param>
    public UnsubscribeFromAnimalHandler(IAnimalService animalService)
    {
        this.animalService = animalService ?? throw new ArgumentNullException(nameof(animalService));
    }

    /// <inheritdoc/>
    public async Task<UnsubscribeResultDto> Handle(UnsubscribeFromAnimalCommand request, CancellationToken cancellationToken)
    {
        await this.animalService.UnsubscribeUserAsync(request.AnimalId, request.UserId, cancellationToken);

        return new UnsubscribeResultDto("Ви успішно відписані від тварини.");
    }
}

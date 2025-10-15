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
    private readonly IAnimalRepository animalRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsubscribeFromAnimalHandler"/> class.
    /// </summary>
    /// <param name="animalRepository">The animal repository.</param>
    public UnsubscribeFromAnimalHandler(IAnimalRepository animalRepository)
    {
        this.animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
    }

    /// <inheritdoc/>
    public async Task<UnsubscribeResultDto> Handle(UnsubscribeFromAnimalCommand request, CancellationToken cancellationToken)
    {
        await this.animalRepository.UnsubscribeUserAsync(request.AnimalId, request.UserId, cancellationToken);

        return new UnsubscribeResultDto("Ви успішно відписані від тварини.");
    }
}

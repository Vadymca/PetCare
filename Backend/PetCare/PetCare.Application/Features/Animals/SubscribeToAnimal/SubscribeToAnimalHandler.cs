namespace PetCare.Application.Features.Animals.SubscribeToAnimal;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// XHandles the subscription of a user to an animal.
/// </summary>
public class SubscribeToAnimalHandler : IRequestHandler<SubscribeToAnimalCommand, AnimalSubscriptionDto>
{
    private readonly IAnimalRepository animalRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribeToAnimalHandler"/> class.
    /// </summary>
    /// <param name="animalRepository">The animal repository.</param>
    public SubscribeToAnimalHandler(IAnimalRepository animalRepository)
    {
        this.animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
    }

    /// <inheritdoc/>
    public async Task<AnimalSubscriptionDto> Handle(SubscribeToAnimalCommand request, CancellationToken cancellationToken)
    {
        await this.animalRepository.SubscribeUserAsync(request.AnimalId, request.UserId, cancellationToken);
        return new AnimalSubscriptionDto(Guid.Empty, request.UserId, request.AnimalId, DateTime.UtcNow);
    }
}

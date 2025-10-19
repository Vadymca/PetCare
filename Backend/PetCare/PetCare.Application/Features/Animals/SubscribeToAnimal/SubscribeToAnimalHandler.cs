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
    private readonly IAnimalService animalService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribeToAnimalHandler"/> class.
    /// </summary>
    /// <param name="animalService">The animal service.</param>
    public SubscribeToAnimalHandler(IAnimalService animalService)
    {
        this.animalService = animalService ?? throw new ArgumentNullException(nameof(animalService));
    }

    /// <inheritdoc/>
    public async Task<AnimalSubscriptionDto> Handle(SubscribeToAnimalCommand request, CancellationToken cancellationToken)
    {
       var subscription = await this.animalService.SubscribeUserAsync(request.AnimalId, request.UserId, cancellationToken);

       return new AnimalSubscriptionDto(
            subscription.Id,
            subscription.UserId,
            subscription.AnimalId,
            DateTime.UtcNow);
    }
}

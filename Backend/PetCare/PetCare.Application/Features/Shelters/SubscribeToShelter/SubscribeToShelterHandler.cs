namespace PetCare.Application.Features.Shelters.SubscribeToShelter;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the subscription of a user to a shelter.
/// </summary>
public sealed class SubscribeToShelterHandler : IRequestHandler<SubscribeToShelterCommand, ShelterSubscriptionDto>
{
    private readonly IShelterService shelterService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribeToShelterHandler"/> class.
    /// </summary>
    /// <param name="shelterService">The shelter service used to manage subscriptions.</param>
    public SubscribeToShelterHandler(
        IShelterService shelterService)
    {
        this.shelterService = shelterService ?? throw new ArgumentNullException(nameof(shelterService));
    }

    /// <inheritdoc/>
    public async Task<ShelterSubscriptionDto> Handle(SubscribeToShelterCommand request, CancellationToken cancellationToken)
    {
        var subscription = await this.shelterService.SubscribeUserAsync(request.ShelterId, request.UserId, cancellationToken);

        return new ShelterSubscriptionDto(
            subscription.Id,
            subscription.UserId,
            subscription.ShelterId,
            DateTime.UtcNow);
    }
}

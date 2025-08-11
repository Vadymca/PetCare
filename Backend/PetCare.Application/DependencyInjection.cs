namespace PetCare.Application;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Application.Abstractions.Events;
using PetCare.Application.EventHandlers.AdoptionApplications;
using PetCare.Application.EventHandlers.Animals;
using PetCare.Application.EventHandlers.Shelters;
using PetCare.Application.EventHandlers.Species;
using PetCare.Application.EventHandlers.VolunteerTasks;
using PetCare.Domain.Events;

/// <summary>
/// Configures dependencies for the Application layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Application-layer services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventHandler<AdoptionApplicationCreatedEvent>, AdoptionApplicationCreatedEventHandler>();
        services.AddScoped<IDomainEventHandler<AdoptionApplicationApprovedEvent>, AdoptionApplicationApprovedEventHandler>();
        services.AddScoped<IDomainEventHandler<AdoptionApplicationRejectedEvent>, AdoptionApplicationRejectedEventHandler>();
        services.AddScoped<IDomainEventHandler<AdoptionApplicationNotesUpdatedEvent>, AdoptionApplicationNotesUpdatedEventHandler>();

        services.AddScoped<IDomainEventHandler<AnimalCreatedEvent>, AnimalCreatedEventHandler>();
        services.AddScoped<IDomainEventHandler<AnimalPhotoAddedEvent>, AnimalPhotoAddedEventHandler>();
        services.AddScoped<IDomainEventHandler<AnimalPhotoRemovedEvent>, AnimalPhotoRemovedEventHandler>();
        services.AddScoped<IDomainEventHandler<AnimalStatusChangedEvent>, AnimalStatusChangedEventHandler>();
        services.AddScoped<IDomainEventHandler<AnimalVideoAddedEvent>, AnimalVideoAddedEventHandler>();
        services.AddScoped<IDomainEventHandler<AnimalVideoRemovedEvent>, AnimalVideoRemovedEventHandler>();

        services.AddScoped<IDomainEventHandler<AnimalAddedToShelterEvent>, AnimalAddedToShelterEventHandler>();
        services.AddScoped<IDomainEventHandler<AnimalRemovedFromShelterEvent>, AnimalRemovedFromShelterEventHandler>();
        services.AddScoped<IDomainEventHandler<ShelterCreatedEvent>, ShelterCreatedEventHandler>();
        services.AddScoped<IDomainEventHandler<ShelterPhotoAddedEvent>, ShelterPhotoAddedEventHandler>();
        services.AddScoped<IDomainEventHandler<ShelterPhotoRemovedEvent>, ShelterPhotoRemovedEventHandler>();
        services.AddScoped<IDomainEventHandler<ShelterSocialMediaAddedOrUpdatedEvent>, ShelterSocialMediaAddedOrUpdatedEventHandler>();
        services.AddScoped<IDomainEventHandler<ShelterSocialMediaRemovedEvent>, ShelterSocialMediaRemovedEventHandler>();
        services.AddScoped<IDomainEventHandler<ShelterUpdatedEvent>, ShelterUpdatedEventHandler>();

        services.AddScoped<IDomainEventHandler<SpecieCreatedEvent>, SpecieCreatedEventHandler>();
        services.AddScoped<IDomainEventHandler<SpecieRenamedEvent>, SpecieRenamedEventHandler>();
        services.AddScoped<IDomainEventHandler<BreedAddedEvent>, BreedAddedEventHandler>();
        services.AddScoped<IDomainEventHandler<BreedRemovedEvent>, BreedRemovedEventHandler>();

        services.AddScoped<IDomainEventHandler<VolunteerTaskCreatedEvent>, VolunteerTaskCreatedEventHandler>();
        services.AddScoped<IDomainEventHandler<VolunteerTaskStatusUpdatedEvent>, VolunteerTaskStatusUpdatedEventHandler>();
        services.AddScoped<IDomainEventHandler<VolunteerTaskInfoUpdatedEvent>, VolunteerTaskInfoUpdatedEventHandler>();
        services.AddScoped<IDomainEventHandler<VolunteerTaskSkillAddedOrUpdatedEvent>, VolunteerTaskSkillAddedOrUpdatedEventHandler>();
        services.AddScoped<IDomainEventHandler<VolunteerTaskSkillRemovedEvent>, VolunteerTaskSkillRemovedEventHandler>();

        return services;
    }
}
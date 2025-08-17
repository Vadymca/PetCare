namespace PetCare.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Domain.Abstractions.Repositories;
<<<<<<< HEAD
using PetCare.Infrastructure.Persistence.Logging;
using PetCare.Infrastructure.Persistence.Notifications;
using PetCare.Infrastructure.Persistence.Repositories;


=======
using PetCare.Infrastructure.Persistence.Repositories;

>>>>>>> 5e60776 (Implementation of repositories for aggregates)
/// <summary>
/// Configures dependencies for the Infrastructure layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Infrastructure-layer services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IAnimalRepository, AnimalRepository>();
<<<<<<< HEAD
        services.AddScoped<IBreedRepository, BreedRepository>();
        services.AddScoped<IShelterRepository, ShelterRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IAuditLogger, AuditLogger>();
        services.AddScoped<INotificationService, NotificationService>();
=======
        services.AddScoped<IAdoptionApplicationRepository, AdoptionApplicationRepository>();
        services.AddScoped<IShelterRepository, ShelterRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVolunteerTaskRepository, VolunteerTaskRepository>();
>>>>>>> 5e60776 (Implementation of repositories for aggregates)

        return services;
    }
}

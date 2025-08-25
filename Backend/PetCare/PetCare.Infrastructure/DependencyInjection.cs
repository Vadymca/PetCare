namespace PetCare.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Abstractions.Services;
using PetCare.Infrastructure.Persistence.Repositories;
using PetCare.Infrastructure.Services.Identity;

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
        services.AddScoped<IShelterRepository, ShelterRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAdoptionApplicationRepository, AdoptionApplicationRepository>();
        services.AddScoped<IVolunteerTaskRepository, VolunteerTaskRepository>();

        // Додаємо AuthorizationService
        services.AddScoped<IAuthorizationService, AuthorizationService>();

        return services;
    }
}

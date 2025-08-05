// <copyright file="DependencyInjection.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Domain.DomainServices;
using PetCare.Domain.Interfaces;
using PetCare.Infrastructure.Persistence.Logging;
using PetCare.Infrastructure.Persistence.Notifications;
using PetCare.Infrastructure.Persistence.Repositories;


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
        services.AddScoped<IBreedRepository, BreedRepository>();
        services.AddScoped<IShelterRepository, ShelterRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IAuditLogger, AuditLogger>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}

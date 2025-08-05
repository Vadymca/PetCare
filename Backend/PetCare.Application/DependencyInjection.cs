// <copyright file="DependencyInjection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PetCare.Application;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Application.Common;
using PetCare.Application.EventHandlers;

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
        services.AddScoped<IDomainEventHandler, AdoptionApplicationCreatedHandler>();
        services.AddScoped<IDomainEventHandler, AdoptionApplicationApprovedHandler>();
        services.AddScoped<IDomainEventHandler, AdoptionApplicationRejectedHandler>();
        services.AddScoped<IDomainEventHandler, AdoptionApplicationNotesUpdatedHandler>();

        return services;
    }
}

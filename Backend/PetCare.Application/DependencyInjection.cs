namespace PetCare.Application;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Application.Abstractions.Events;
using System.Reflection;

/// <summary>
/// Configures dependencies for the Application layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Application-layer services.
    /// </summary>
    /// <param name="services">The service collection to which Application services will be added.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services.RegisterAllDomainEventHandlers();
        return services;
    }

    /// <summary>
    /// Automatically registers all domain event handlers from the current assembly.
    /// </summary>
    private static IServiceCollection RegisterAllDomainEventHandlers(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var handlerInterfaceType = typeof(IDomainEventHandler<>);

        var handlers = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
                .Select(i => new { Interface = i, Implementation = t }))
            .Distinct();

        foreach (var handler in handlers)
        {
            services.AddScoped(handler.Interface, handler.Implementation);
        }

        return services;
    }
}

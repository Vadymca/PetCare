namespace PetCare.Application;

using Microsoft.Extensions.DependencyInjection;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Abstractions.Events;
using System.Reflection;

/// <summary>
/// Configures dependencies for the Application layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Application-layer services, including MediatR handlers and domain event dispatcher.
    /// </summary>
    /// <param name="services">The service collection to which application services are added.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Реєструємо всі INotificationHandler<T> з поточної збірки
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Реєструємо Dispatcher, який використовує MediatR
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}

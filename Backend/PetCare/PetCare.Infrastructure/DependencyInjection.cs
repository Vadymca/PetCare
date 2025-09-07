namespace PetCare.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Abstractions.Services;
using PetCare.Infrastructure.Options;
using PetCare.Infrastructure.Persistence.Repositories;
using PetCare.Infrastructure.Services;
using PetCare.Infrastructure.Services.Email;
using PetCare.Infrastructure.Services.Identity;

/// <summary>
/// Configures dependencies for the Infrastructure layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Infrastructure-layer services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();

        // Repositories
        services.AddScoped<IAnimalRepository, AnimalRepository>();
        services.AddScoped<IShelterRepository, ShelterRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAdoptionApplicationRepository, AdoptionApplicationRepository>();
        services.AddScoped<IVolunteerTaskRepository, VolunteerTaskRepository>();

        // Domain services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IQrCodeGenerator, QrCodeGeneratorService>();

        // Email services
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IEmailTemplateRenderer, EmailTemplateRenderer>();

        // Sms and Twilio services
        services.Configure<SmsSettings>(configuration.GetSection("SmsSettings"));
        services.Configure<TwilioSettings>(configuration.GetSection("TwilioSettings"));
        services.AddTransient<ISmsService, TwilioSmsService>();
        services.AddScoped<ISms2FaService, Sms2FaService>();

        return services;
    }
}

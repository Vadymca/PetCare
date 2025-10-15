namespace PetCare.Infrastructure;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PetCare.Application.Interfaces;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Abstractions.Services;
using PetCare.Infrastructure.Options;
using PetCare.Infrastructure.Persistence.Repositories;
using PetCare.Infrastructure.Services;
using PetCare.Infrastructure.Services.Email;
using PetCare.Infrastructure.Services.Identity;
using PetCare.Infrastructure.Services.Sms;
using PetCare.Infrastructure.Services.Zipcodebase;

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
        services.AddScoped<IEmailAssetProvider, FileEmailAssetProvider>();

        // Sms and Twilio services
        services.Configure<SmsSettings>(configuration.GetSection("SmsSettings"));
        services.Configure<TwilioSettings>(configuration.GetSection("TwilioSettings"));
        services.AddScoped<ISms2FaService, Sms2FaService>();
        services.Configure<SmsFlySettings>(configuration.GetSection("SmsFly"));
        services.AddHttpClient<ISmsService, SmsFlyService>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<SmsFlySettings>>().Value;
            client.BaseAddress = new Uri("https://sms-fly.ua/");
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("User-Agent", "PetCare/1.0");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            UseCookies = false,
        });

        // Zipcodebase service
        services.Configure<ZipcodebaseOptions>(configuration.GetSection(ZipcodebaseOptions.SectionName));

        services.AddHttpClient<IZipcodebaseService, ZipcodebaseService>(client =>
        {
            client.DefaultRequestHeaders.Add("User-Agent", "PetCareApp/1.0");
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(10),
            UseProxy = false,
            AllowAutoRedirect = true,
            MaxConnectionsPerServer = 10,
        });

        // FileStorage service
        services.AddSingleton<IFileStorageService>(sp =>
        {
            var env = sp.GetRequiredService<IWebHostEnvironment>();
            return new FileStorageService(env.WebRootPath);
        });

        // Facebook OAuth settings
        services.Configure<FacebookSettings>(
            configuration.GetSection("Facebook"));
        services.AddScoped<IFacebookAuthService, FacebookAuthService>();

        // Google OAuth settings
        services.Configure<GoogleSettings>(
            configuration.GetSection("Google"));
        services.AddHttpClient<IGoogleAuthService, GoogleAuthService>();

        // Minio Storage service
        services.AddScoped<IStorageService, MinioStorageService>();

        return services;
    }
}

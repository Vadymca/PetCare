namespace PetCare.Infrastructure.Data;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetCare.Infrastructure.Identity;

/// <summary>
/// Provides data seeding functionality for the application.
/// </summary>
public static class DataSeeder
{
    /// <summary>
    /// Seeds initial data including roles.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(typeof(DataSeeder));

        await SeedRolesAsync(roleManager, logger);
    }

    /// <summary>
    /// Seeds the default roles.
    /// </summary>
    /// <param name="roleManager">The role manager.</param>
    /// <param name="logger">The logger.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private static async Task SeedRolesAsync(RoleManager<AppRole> roleManager, ILogger logger)
    {
        logger.LogInformation("Seeding roles...");

        var roles = new[]
        {
        "User",
        "Admin",
        "ShelterManager",
        "Veterinarian",
        "Volunteer",
        };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new AppRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpperInvariant(),
                };

                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    logger.LogInformation("Role '{RoleName}' created successfully.", roleName);
                }
                else
                {
                    logger.LogError(
                        "Failed to create role '{RoleName}': {Errors}",
                        roleName,
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogInformation("Role '{RoleName}' already exists.", roleName);
            }
        }

        logger.LogInformation("Roles seeding completed.");
    }
}

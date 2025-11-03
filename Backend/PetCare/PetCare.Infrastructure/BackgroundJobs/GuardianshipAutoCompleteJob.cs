namespace PetCare.Infrastructure.BackgroundJobs;

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;

/// <summary>
/// Background job that periodically completes expired guardianships.
/// </summary>
/// <remarks>
/// This job runs automatically on a schedule and marks guardianships whose grace period has expired
/// as completed. It uses a scoped lifetime for all database-related services.
/// </remarks>
public sealed class GuardianshipAutoCompleteJob : BackgroundService
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly ILogger<GuardianshipAutoCompleteJob> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuardianshipAutoCompleteJob"/> class.
    /// </summary>
    /// <param name="scopeFactory">The service scope factory used to create scoped dependencies.</param>
    /// <param name="logger">The logger used to record job execution details.</param>
    public GuardianshipAutoCompleteJob(
        IServiceScopeFactory scopeFactory,
        ILogger<GuardianshipAutoCompleteJob> logger)
    {
        this.scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("GuardianshipAutoCompleteJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = this.scopeFactory.CreateScope();
            var guardianships = scope.ServiceProvider.GetRequiredService<IGuardianshipService>();

            try
            {
                var completedCount = await guardianships.AutoCompleteExpiredAsync(DateTime.UtcNow, stoppingToken);
                if (completedCount > 0)
                {
                    this.logger.LogInformation("Auto-completed {Count} expired guardianships at {Time}.", completedCount, DateTime.UtcNow);
                }
                else
                {
                    this.logger.LogDebug("No expired guardianships to complete at {Time}.", DateTime.UtcNow);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while auto-completing expired guardianships.");
            }

            // Run every 24 hours (tune this interval as needed)
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }

        this.logger.LogInformation("GuardianshipAutoCompleteJob stopped.");
    }
}
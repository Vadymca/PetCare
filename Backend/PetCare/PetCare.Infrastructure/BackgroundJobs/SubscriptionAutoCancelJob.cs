namespace PetCare.Infrastructure.BackgroundJobs;

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetCare.Application.Interfaces;

/// <summary>
/// Background job that periodically cancels expired or invalid subscriptions.
/// </summary>
/// <remarks>
/// This job checks all active subscriptions and cancels those that have expired or failed to renew.
/// It runs in a loop with a fixed delay and creates a scoped context for each iteration.
/// </remarks>
public sealed class SubscriptionAutoCancelJob : BackgroundService
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly ILogger<SubscriptionAutoCancelJob> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionAutoCancelJob"/> class.
    /// </summary>
    /// <param name="scopeFactory">The service scope factory used to create scoped dependencies.</param>
    /// <param name="logger">The logger used to record job execution details.</param>
    public SubscriptionAutoCancelJob(
        IServiceScopeFactory scopeFactory,
        ILogger<SubscriptionAutoCancelJob> logger)
    {
        this.scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("SubscriptionAutoCancelJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = this.scopeFactory.CreateScope();
            var subscriptionService = scope.ServiceProvider.GetRequiredService<ISubscriptionService>();

            try
            {
                var canceledCount = await subscriptionService.CancelExpiredAsync(stoppingToken);
                if (canceledCount > 0)
                {
                    this.logger.LogInformation("Auto-canceled {Count} expired subscriptions at {Time}.", canceledCount, DateTime.UtcNow);
                }
                else
                {
                    this.logger.LogDebug("No expired subscriptions to cancel at {Time}.", DateTime.UtcNow);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while auto-canceling subscriptions.");
            }

            // Run every 24 hours
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }

        this.logger.LogInformation("SubscriptionAutoCancelJob stopped.");
    }
}
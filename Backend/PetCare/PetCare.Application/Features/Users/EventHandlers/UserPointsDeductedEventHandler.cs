namespace PetCare.Application.Features.Users.EventHandlers;

using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Domain.Events;

/// <summary>
/// Handles <see cref="UserPointsDeductedEvent"/>.
/// Logs when points are deducted from a user.
/// </summary>
public sealed class UserPointsDeductedEventHandler : INotificationHandler<UserPointsDeductedEvent>
{
    private readonly ILogger<UserPointsDeductedEventHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserPointsDeductedEventHandler"/> class.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    public UserPointsDeductedEventHandler(ILogger<UserPointsDeductedEventHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task Handle(UserPointsDeductedEvent notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation(
            "Deducted {Amount} points from user {UserId}.",
            notification.Amount,
            notification.UserId);

        await Task.CompletedTask;
    }
}

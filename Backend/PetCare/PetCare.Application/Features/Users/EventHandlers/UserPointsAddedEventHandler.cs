namespace PetCare.Application.Features.Users.EventHandlers;

using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Domain.Events;

/// <summary>
/// Handles <see cref="UserPointsAddedEvent"/>.
/// Logs when points are added to a user.
/// </summary>
public sealed class UserPointsAddedEventHandler : INotificationHandler<UserPointsAddedEvent>
{
    private readonly ILogger<UserPointsAddedEventHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserPointsAddedEventHandler"/> class.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    public UserPointsAddedEventHandler(ILogger<UserPointsAddedEventHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task Handle(UserPointsAddedEvent notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation(
            "Added {Amount} points to user {UserId}.",
            notification.Amount,
            notification.UserId);

        await Task.CompletedTask;
    }
}
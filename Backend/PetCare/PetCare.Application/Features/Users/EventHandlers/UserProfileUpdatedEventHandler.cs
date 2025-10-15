namespace PetCare.Application.Features.Users.EventHandlers;

using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Domain.Events;

/// <summary>
/// Handles <see cref="UserProfileUpdatedEvent"/>.
/// Logs information when a user's profile is updated.
/// </summary>
public sealed class UserProfileUpdatedEventHandler : INotificationHandler<UserProfileUpdatedEvent>
{
    private readonly ILogger<UserProfileUpdatedEventHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserProfileUpdatedEventHandler"/> class.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    public UserProfileUpdatedEventHandler(ILogger<UserProfileUpdatedEventHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task Handle(UserProfileUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Log that the user's profile was updated
        this.logger.LogInformation("User profile updated. UserId: {UserId}", notification.UserId);

        await Task.CompletedTask;
    }
}

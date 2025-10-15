namespace PetCare.Application.Features.Auth.EventHandlers;

using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Domain.Events;

/// <summary>
/// Handles UserPasswordChangedEvent.
/// </summary>
public class UserPasswordChangedEventHandler : INotificationHandler<UserPasswordChangedEvent>
{
    private readonly ILogger<UserPasswordChangedEventHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserPasswordChangedEventHandler"/> class.
    /// </summary>
    /// /// <param name="logger">
    /// The logger instance used to record diagnostic and operational messages.
    /// </param>
    public UserPasswordChangedEventHandler(ILogger<UserPasswordChangedEventHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task Handle(UserPasswordChangedEvent notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("User with ID {UserId} has changed their password.", notification.UserId);
        await Task.CompletedTask;
    }
}

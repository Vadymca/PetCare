namespace PetCare.Application.Features.Auth.EventHandlers;

using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Domain.Events;

/// <summary>
/// Handles UserLastLoginSetEvent.
/// </summary>
public sealed class UserLastLoginSetEventHandler : INotificationHandler<UserLastLoginSetEvent>
{
    private readonly string message = "Подія UserLastLoginSetEvent спрацювала для користувача {UserId} з часом {LastLogin}";
    private readonly ILogger<UserLastLoginSetEventHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserLastLoginSetEventHandler"/> class.
    /// </summary>
    /// <param name="logger">Logger for diagnostic messages.</param>
    public UserLastLoginSetEventHandler(ILogger<UserLastLoginSetEventHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task Handle(UserLastLoginSetEvent notification, CancellationToken cancellationToken)
    {
        var handlerInstanceId = Guid.NewGuid().ToString("N")[..8];

        // Логування події
        this.logger.LogInformation(
            this.message,
            handlerInstanceId,
            notification.UserId,
            notification.LastLogin);

        await Task.CompletedTask;
    }
}

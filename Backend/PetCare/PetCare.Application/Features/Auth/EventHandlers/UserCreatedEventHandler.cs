namespace PetCare.Application.Features.Auth.EventHandlers;

using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Domain.Events;

/// <summary>
/// Handles UserCreatedEvent.
/// </summary>
public sealed class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserCreatedEventHandler"/> class.
    /// </summary>
    /// /// <param name="logger">
    /// The logger instance used to record diagnostic and operational messages.
    /// </param>
    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Логування для перевірки отримання події
        this.logger.LogInformation(
            "UserCreatedEvent received for UserId={UserId}",
            notification.UserId);

        // Тут можна додати додаткову логіку:
        // - Відправка welcome email
        // - Створення початкових налаштувань
        // - Логування в аналітику
        // - Інтеграція з зовнішніми сервісами
        return Task.CompletedTask;
    }
}

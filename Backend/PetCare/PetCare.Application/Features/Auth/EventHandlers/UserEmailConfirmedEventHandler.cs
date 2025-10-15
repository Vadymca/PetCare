namespace PetCare.Application.Features.Auth.EventHandlers;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Domain.Events;

/// <summary>
/// Handles <see cref="UserEmailConfirmedEvent"/>.
/// Logs that a user's email has been successfully confirmed.
/// </summary>
public sealed class UserEmailConfirmedEventHandler : INotificationHandler<UserEmailConfirmedEvent>
{
    private readonly ILogger<UserEmailConfirmedEventHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserEmailConfirmedEventHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger instance used to record diagnostic messages.</param>
    public UserEmailConfirmedEventHandler(ILogger<UserEmailConfirmedEventHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task Handle(UserEmailConfirmedEvent notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation(
            "User email confirmed: UserId={UserId}, Email={Email}",
            notification.UserId,
            notification.Email);

        await Task.CompletedTask;
    }
}

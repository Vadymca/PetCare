// <copyright file="AdoptionApplicationNotesUpdatedHandler.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Application.EventHandlers;
using PetCare.Application.Common;
using PetCare.Domain.DomainServices;
using PetCare.Domain.Events;

/// <summary>
/// Handles the event raised when admin notes are updated on an adoption application.
/// </summary>
public sealed class AdoptionApplicationNotesUpdatedHandler : IDomainEventHandler
{
    private readonly IAuditLogger auditLogger;
    private readonly INotificationService notificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdoptionApplicationNotesUpdatedHandler"/> class.
    /// </summary>
    /// <param name="auditLogger">The audit logger used to log admin actions.</param>
    /// <param name="notificationService">The service used to notify moderators.</param>
    public AdoptionApplicationNotesUpdatedHandler(
        IAuditLogger auditLogger,
        INotificationService notificationService)
    {
        this.auditLogger = auditLogger;
        this.notificationService = notificationService;
    }

    /// <inheritdoc/>
    public async Task Handle(DomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var @event = (AdoptionApplicationNotesUpdatedEvent)domainEvent;

        var logMessage = $"Адміністратор оновив примітки до заявки {@event.applicationId}, поданої користувачем {@event.userId}. Зміст приміток: '{@event.notes}'.";
        await this.auditLogger.LogAsync(logMessage, cancellationToken);

        var notificationSubject = "Оновлено примітки адміністратора до заявки на адопцію";
        var notificationBody = $"ID заявки: {@event.applicationId}\nID користувача: {@event.userId}\nПримітки: {@event.notes}";
        await this.notificationService.NotifyModeratorsAsync(notificationSubject, notificationBody, cancellationToken);
    }
}

namespace PetCare.Application.EventHandlers.AdoptionApplications;

using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Abstractions.Repositories;
using PetCare.Domain.Events;

/// <summary>
/// Handles the event raised when admin notes are updated on an adoption application.
/// </summary>
public sealed class AdoptionApplicationNotesUpdatedHandler : IDomainEventHandler<AdoptionApplicationNotesUpdatedEvent>
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
    public async Task HandleAsync(AdoptionApplicationNotesUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var logMessage = $"Адміністратор оновив примітки до заявки {domainEvent.applicationId}, поданої користувачем {domainEvent.userId}. Зміст приміток: '{domainEvent.notes}'.";
        await this.auditLogger.LogAsync(logMessage, cancellationToken);

        var notificationSubject = "Оновлено примітки адміністратора до заявки на адопцію";
        var notificationBody = $"ID заявки: {domainEvent.applicationId}\nID користувача: {domainEvent.userId}\nПримітки: {domainEvent.notes}";
        await this.notificationService.NotifyModeratorsAsync(notificationSubject, notificationBody, cancellationToken);
    }
}

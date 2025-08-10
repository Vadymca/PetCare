namespace PetCare.Domain.Events;

/// <summary>
/// Event raised when a volunteer task is assigned to a user.
/// </summary>
public sealed record VolunteerTaskAssignedEvent(Guid volunteerTaskId, Guid userId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

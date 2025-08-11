namespace PetCare.Domain.Events;
using PetCare.Domain.Enums;
using System;

public sealed record VolunteerTaskCreatedEvent(Guid volunteerTaskId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
public sealed record VolunteerTaskStatusUpdatedEvent(Guid volunteerTaskId, VolunteerTaskStatus newStatus)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
public sealed record VolunteerTaskInfoUpdatedEvent(Guid volunteerTaskId)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
public sealed record VolunteerTaskSkillAddedOrUpdatedEvent(Guid volunteerTaskId, string skillName, string description)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);
public sealed record VolunteerTaskSkillRemovedEvent(Guid volunteerTaskId, string skillName)
    : DomainEvent(Guid.NewGuid(), DateTime.UtcNow);

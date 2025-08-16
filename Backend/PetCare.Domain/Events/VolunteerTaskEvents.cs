namespace PetCare.Domain.Events;
using PetCare.Domain.Enums;
using System;

public sealed record VolunteerTaskCreatedEvent(Guid volunteerTaskId)
    : DomainEvent;

public sealed record VolunteerTaskStatusUpdatedEvent(Guid volunteerTaskId, VolunteerTaskStatus newStatus)
    : DomainEvent;

public sealed record VolunteerTaskInfoUpdatedEvent(Guid volunteerTaskId)
    : DomainEvent;

public sealed record VolunteerTaskSkillAddedOrUpdatedEvent(Guid volunteerTaskId, string skillName, string description)
    : DomainEvent;

public sealed record VolunteerTaskSkillRemovedEvent(Guid volunteerTaskId, string skillName)
    : DomainEvent;

public record VolunteerTaskAssignmentAddedEvent(Guid taskId, Guid assignmentId)
      : DomainEvent;

public record VolunteerTaskAssignmentRemovedEvent(Guid taskId, Guid assignmentId)
    : DomainEvent;

public record VolunteerTaskRewardAddedEvent(Guid taskId, Guid rewardId)
    : DomainEvent;

public record VolunteerTaskRewardRemovedEvent(Guid taskId, Guid rewardId)
    : DomainEvent;
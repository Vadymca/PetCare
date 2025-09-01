namespace PetCare.Domain.Events;
using PetCare.Domain.Enums;
using System;

public sealed record VolunteerTaskCreatedEvent(Guid VolunteerTaskId)
    : DomainEvent;

public sealed record VolunteerTaskStatusUpdatedEvent(Guid VolunteerTaskId, VolunteerTaskStatus NewStatus)
    : DomainEvent;

public sealed record VolunteerTaskInfoUpdatedEvent(Guid VolunteerTaskId)
    : DomainEvent;

public sealed record VolunteerTaskSkillAddedOrUpdatedEvent(Guid VolunteerTaskId, string SkillName, string Description)
    : DomainEvent;

public sealed record VolunteerTaskSkillRemovedEvent(Guid VolunteerTaskId, string SkillName)
    : DomainEvent;

public record VolunteerTaskAssignmentAddedEvent(Guid TaskId, Guid AssignmentId)
      : DomainEvent;

public record VolunteerTaskAssignmentRemovedEvent(Guid TaskId, Guid AssignmentId)
    : DomainEvent;

public record VolunteerTaskRewardAddedEvent(Guid TaskId, Guid RewardId)
    : DomainEvent;

public record VolunteerTaskRewardRemovedEvent(Guid TaskId, Guid RewardId)
    : DomainEvent;
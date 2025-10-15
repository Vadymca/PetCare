namespace PetCare.Domain.Events;

using System;
using PetCare.Domain.Enums;

/// <summary>
/// Represents an event that occurs when a new volunteer task is created.
/// </summary>
/// <param name="VolunteerTaskId">The unique identifier of the volunteer task associated with this event.</param>
public sealed record VolunteerTaskCreatedEvent(Guid VolunteerTaskId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when the status of a volunteer task is updated.
/// </summary>
/// <param name="VolunteerTaskId">The unique identifier of the volunteer task whose status has changed.</param>
/// <param name="NewStatus">The new status assigned to the volunteer task.</param>
public sealed record VolunteerTaskStatusUpdatedEvent(Guid VolunteerTaskId, VolunteerTaskStatus NewStatus)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when the information for a volunteer task is updated.
/// </summary>
/// <param name="VolunteerTaskId">The unique identifier of the volunteer task whose information has been updated.</param>
public sealed record VolunteerTaskInfoUpdatedEvent(Guid VolunteerTaskId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a skill is added to or updated for a volunteer task.
/// </summary>
/// <param name="VolunteerTaskId">The unique identifier of the volunteer task to which the skill is associated.</param>
/// <param name="SkillName">The name of the skill that was added or updated.</param>
/// <param name="Description">A description of the skill that was added or updated.</param>
public sealed record VolunteerTaskSkillAddedOrUpdatedEvent(Guid VolunteerTaskId, string SkillName, string Description)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a skill is removed from a volunteer task.
/// </summary>
/// <param name="VolunteerTaskId">The unique identifier of the volunteer task from which the skill was removed.</param>
/// <param name="SkillName">The name of the skill that was removed from the volunteer task.</param>
public sealed record VolunteerTaskSkillRemovedEvent(Guid VolunteerTaskId, string SkillName)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a volunteer is assigned to a task.
/// </summary>
/// <param name="TaskId">The unique identifier of the task to which the volunteer is assigned.</param>
/// <param name="AssignmentId">The unique identifier of the assignment created for the volunteer.</param>
public record VolunteerTaskAssignmentAddedEvent(Guid TaskId, Guid AssignmentId)
      : DomainEvent;

/// <summary>
/// Represents an event that occurs when a volunteer's assignment is removed from a task.
/// </summary>
/// <param name="TaskId">The unique identifier of the task from which the assignment was removed.</param>
/// <param name="AssignmentId">The unique identifier of the volunteer assignment that was removed.</param>
public record VolunteerTaskAssignmentRemovedEvent(Guid TaskId, Guid AssignmentId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a reward is added to a volunteer task.
/// </summary>
/// <param name="TaskId">The unique identifier of the volunteer task to which the reward was added.</param>
/// <param name="RewardId">The unique identifier of the reward that was added to the task.</param>
public record VolunteerTaskRewardAddedEvent(Guid TaskId, Guid RewardId)
    : DomainEvent;

/// <summary>
/// Represents an event that occurs when a reward is removed from a volunteer task.
/// </summary>
/// <param name="TaskId">The unique identifier of the volunteer task from which the reward was removed.</param>
/// <param name="RewardId">The unique identifier of the reward that was removed from the task.</param>
public record VolunteerTaskRewardRemovedEvent(Guid TaskId, Guid RewardId)
    : DomainEvent;
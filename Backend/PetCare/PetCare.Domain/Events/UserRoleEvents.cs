namespace PetCare.Domain.Events;
using PetCare.Domain.ValueObjects;
using System;

/// <summary>
/// Raised when a role is assigned to a user.
/// </summary>
public record UserRoleAssignedEvent(Guid UserId, Role Role)
    : DomainEvent;

/// <summary>
/// Raised when a role is removed from a user.
/// </summary>
public record UserRoleRemovedEvent(Guid UserId, Role Role)
    : DomainEvent;

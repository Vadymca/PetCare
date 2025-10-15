namespace PetCare.Domain.Abstractions.Services;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Provides asynchronous operations for managing volunteer task assignments for users.
/// </summary>
public interface IVolunteerTaskAssignmentService
{
    /// <summary>
    /// Adds a new volunteer task assignment for the user.
    /// </summary>
    /// <param name="user">The user who will receive the assignment.</param>
    /// <param name="assignment">The assignment to add.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation. Must match the owner or be an admin/moderator.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddVolunteerTaskAssignmentAsync(User user, VolunteerTaskAssignment assignment, Guid requestingUserId);

    /// <summary>
    /// Removes a volunteer task assignment from the user.
    /// </summary>
    /// <param name="user">The user who owns the assignment.</param>
    /// <param name="assignmentId">The ID of the assignment to remove.</param>
    /// <param name="requestingUserId">The ID of the user performing the operation. Must match the owner or be an admin/moderator.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveVolunteerTaskAssignmentAsync(User user, Guid assignmentId, Guid requestingUserId);
}

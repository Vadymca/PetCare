namespace PetCare.Domain.DomainServices;

using System;
using System.Threading.Tasks;
using PetCare.Domain.Abstractions.Services;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// Provides operations for managing volunteer task assignments for users.
/// </summary>
public sealed class VolunteerTaskAssignmentService : IVolunteerTaskAssignmentService
{
    /// <inheritdoc/>
    public async Task AddVolunteerTaskAssignmentAsync(User user, VolunteerTaskAssignment assignment, Guid requestingUserId)
    {
        user.AddVolunteerTaskAssignment(assignment, requestingUserId);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task RemoveVolunteerTaskAssignmentAsync(User user, Guid assignmentId, Guid requestingUserId)
    {
        user.RemoveVolunteerTaskAssignment(assignmentId, requestingUserId);
        await Task.CompletedTask;
    }
}

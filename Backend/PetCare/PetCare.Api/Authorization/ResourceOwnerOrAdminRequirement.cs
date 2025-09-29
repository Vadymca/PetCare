namespace PetCare.Api.Authorization;

using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Marker requirement for the "ResourceOwnerOrAdmin" policy.
/// This requirement is handled by <see cref="ResourceOwnerOrAdminHandler"/>.
/// Grants access if the user is either an Admin or the owner of the resource.
/// </summary>
public sealed class ResourceOwnerOrAdminRequirement : IAuthorizationRequirement
{
}

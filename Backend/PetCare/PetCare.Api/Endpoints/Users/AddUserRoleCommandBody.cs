namespace PetCare.Api.Endpoints.Users;

/// <summary>
/// Represents the data required to assign a role to a user.
/// </summary>
/// <param name="Role">The name of the role to assign to the user. Cannot be null or empty.</param>
public sealed record AddUserRoleCommandBody(string Role);

namespace PetCare.Infrastructure.Identity;

using System;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Represents a role in the PetCare system for ASP.NET Identity.
/// Inherits from <see cref="IdentityRole{Guid}"/> using a GUID as the primary key.
/// </summary>
public sealed class AppRole : IdentityRole<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppRole"/> class.
    /// Parameterless constructor required by EF Core and Identity.
    /// </summary>
    public AppRole()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppRole"/> class with a specified role name.
    /// </summary>
    /// <param name="name">The name of the role (e.g., "User", "Admin").</param>
    public AppRole(string name)
        : base(name)
    {
    }
}
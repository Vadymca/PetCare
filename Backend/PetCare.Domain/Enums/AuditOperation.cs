// <copyright file="AuditOperation.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the types of operations that can be audited.
/// </summary>
public enum AuditOperation
{
    /// <summary>
    /// Represents an insert operation.
    /// </summary>
    Insert,

    /// <summary>
    /// Represents an update operation.
    /// </summary>
    Update,

    /// <summary>
    /// Represents a delete operation.
    /// </summary>
    Delete,
}

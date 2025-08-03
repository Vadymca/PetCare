// <copyright file="AdoptionStatus.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the status of an adoption application.
/// </summary>
public enum AdoptionStatus
{
    /// <summary>
    /// The application is awaiting review.
    /// </summary>
    Pending,

    /// <summary>
    /// The application has been approved.
    /// </summary>
    Approved,

    /// <summary>
    /// The application has been rejected.
    /// </summary>
    Rejected,
}

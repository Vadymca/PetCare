// <copyright file="AidStatus.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the current status of an aid request.
/// </summary>
public enum AidStatus
{
    /// <summary>
    /// The aid request is open and waiting for a response.
    /// </summary>
    Open,

    /// <summary>
    /// The aid request is currently being processed or handled.
    /// </summary>
    InProgress,

    /// <summary>
    /// The aid request has been successfully fulfilled.
    /// </summary>
    Fulfilled,

    /// <summary>
    /// The aid request has been cancelled and will not be processed.
    /// </summary>
    Cancelled,
}

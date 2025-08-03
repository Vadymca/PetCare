// <copyright file="AnimalStatus.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the current status of an animal in the system.
/// </summary>
public enum AnimalStatus
{
    /// <summary>
    /// The animal is available for adoption.
    /// </summary>
    Available,

    /// <summary>
    /// The animal has been adopted.
    /// </summary>
    Adopted,

    /// <summary>
    /// The animal is reserved for adoption.
    /// </summary>
    Reserved,

    /// <summary>
    /// The animal is currently undergoing treatment.
    /// </summary>
    InTreatment,
}

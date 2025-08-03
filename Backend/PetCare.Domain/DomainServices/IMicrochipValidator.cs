// <copyright file="IMicrochipValidator.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.DomainServices;

/// <summary>
/// Provides validation logic for animal microchips.
/// </summary>
public interface IMicrochipValidator
{
    /// <summary>
    /// Checks whether a microchip ID is unique across the system.
    /// </summary>
    /// <param name="microchipId">The microchip identifier to check.</param>
    /// <returns>A task that resolves to <c>true</c> if the microchip is unique; otherwise, <c>false</c>.</returns>
    Task<bool> IsMicrochipUniqueAsync(string microchipId);

    /// <summary>
    /// Checks whether the format of the microchip ID is valid.
    /// </summary>
    /// <param name="microchipId">The microchip identifier to validate.</param>
    /// <returns><c>true</c> if the format is valid; otherwise, <c>false</c>.</returns>
    bool IsValidFormat(string microchipId);
}

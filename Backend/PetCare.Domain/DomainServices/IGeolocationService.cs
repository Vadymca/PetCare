// <copyright file="IGeolocationService.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.DomainServices;
using NetTopologySuite.Geometries;

/// <summary>
/// Defines a contract for converting between addresses and geolocations.
/// </summary>
public interface IGeolocationService
{
    /// <summary>
    /// Gets the coordinates (point) for a given address.
    /// </summary>
    /// <param name="address">The address to convert to coordinates.</param>
    /// <returns>A task that represents the asynchronous operation, returning the coordinates as a <see cref="Point"/> or null if the address cannot be resolved.</returns>
    Task<Point?> GetCoordinatesFromAddressAsync(string address);

    /// <summary>
    /// Gets the textual address for a given geographic point.
    /// </summary>
    /// <param name="location">The geographic point to convert to an address.</param>
    /// <returns>A task that represents the asynchronous operation, returning the address as a string or null if the location cannot be resolved.</returns>
    Task<string?> GetAddressFromCoordinatesAsync(Point location);
}

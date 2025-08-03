// <copyright file="IImageValidator.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.DomainServices;

/// <summary>
/// Provides image validation before saving or processing.
/// </summary>
public interface IImageValidator
{
    /// <summary>
    /// Checks whether the image meets allowed dimensions and file format.
    /// </summary>
    /// <param name="imageBytes">The image as a byte array.</param>
    /// <param name="fileName">The original file name of the image.</param>
    /// <returns><c>true</c> if the image is valid; otherwise, <c>false</c>.</returns>
    bool IsValid(byte[] imageBytes, string fileName);

    /// <summary>
    /// Returns a list of validation errors if the image is invalid.
    /// </summary>
    /// <param name="imageBytes">The image as a byte array.</param>
    /// <param name="fileName">The original file name of the image.</param>
    /// <returns>A read-only collection of validation error messages.</returns>
    IReadOnlyCollection<string> GetValidationErrors(byte[] imageBytes, string fileName);
}

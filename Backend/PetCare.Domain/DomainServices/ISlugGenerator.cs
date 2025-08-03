// <copyright file="ISlugGenerator.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.DomainServices;

/// <summary>
/// Provides functionality for generating unique slugs for entities.
/// </summary>
public interface ISlugGenerator
{
    /// <summary>
    /// Generates a unique slug for the specified input within the given entity type.
    /// </summary>
    /// <param name="input">The base text from which to generate the slug.</param>
    /// <param name="entityType">The type of entity for which the slug is being generated (e.g. "Article").</param>
    /// <returns>A task that resolves to a unique slug string.</returns>
    Task<string> GenerateSlugAsync(string input, string entityType);
}

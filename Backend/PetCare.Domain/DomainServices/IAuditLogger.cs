// <copyright file="IAuditLogger.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.DomainServices;

/// <summary>
/// Provides logging functionality for auditing domain-level events.
/// </summary>
public interface IAuditLogger
{
    /// <summary>
    /// Logs a message to the audit trail.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task LogAsync(string message, CancellationToken cancellationToken);
}

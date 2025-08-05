// <copyright file="AuditLogger.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure.Persistence.Logging;
using PetCare.Domain.DomainServices;

/// <summary>
/// Provides functionality for asynchronous audit logging.
/// </summary>
public class AuditLogger : IAuditLogger
{
    /// <summary>
    /// Logs a message asynchronously.
    /// </summary>
    /// <param name="message">The audit message to log.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task LogAsync(string message, CancellationToken cancellationToken)
    {
        // Не встиг добавити логіку
        await Task.CompletedTask;
    }
}

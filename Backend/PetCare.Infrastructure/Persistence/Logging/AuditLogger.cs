// <copyright file="AuditLogger.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure.Persistence.Logging;

using PetCare.Domain.DomainServices;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Implements audit logging using Serilog.
/// </summary>
public class AuditLogger : IAuditLogger
{
    private readonly Serilog.ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditLogger"/> class.
    /// </summary>
    public AuditLogger()
    {
        this.logger = Log.ForContext<AuditLogger>();
    }

    /// <inheritdoc />
    public Task LogAsync(
    string message,
    CancellationToken cancellationToken)
    {
        return Task.Run(
            () =>
        {
            this.logger.Information("[AUDIT] {Message}", message);
        }, cancellationToken);
    }
}

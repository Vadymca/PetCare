// <copyright file="IAuditLogWriter.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.DomainServices;
using Newtonsoft.Json.Linq;

/// <summary>
/// Defines a contract for writing audit log entries.
/// </summary>
public interface IAuditLogWriter
{
    /// <summary>
    /// Writes a change to the audit log.
    /// </summary>
    /// <param name="tableName">The name of the table affected by the change.</param>
    /// <param name="recordId">The unique identifier of the record affected by the change.</param>
    /// <param name="operation">The operation performed (e.g., create, update, delete).</param>
    /// <param name="userId">The unique identifier of the user who performed the operation, if available. Can be null.</param>
    /// <param name="changes">The changes made, represented as a JSON object.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task WriteAsync(
        string tableName,
        Guid recordId,
        string operation,
        Guid? userId,
        JObject changes);
}
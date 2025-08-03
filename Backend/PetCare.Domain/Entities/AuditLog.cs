// <copyright file="AuditLog.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Entities;
using PetCare.Domain.Common;
using PetCare.Domain.Enums;

/// <summary>
/// Represents an audit log entry for tracking changes in the database.
/// </summary>
public sealed class AuditLog : BaseEntity
{
    private AuditLog()
    {
    }

    private AuditLog(
        string tableName,
        Guid recordId,
        AuditOperation operation,
        Guid? userId,
        string? changes)
    {
        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new ArgumentException("Назва таблиці не може бути порожньою.", nameof(tableName));
        }

        if (recordId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор запису не може бути порожнім.", nameof(recordId));
        }

        this.TableName = tableName;
        this.RecordId = recordId;
        this.Operation = operation;
        this.UserId = userId;
        this.Changes = changes;
        this.CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the name of the table where the change occurred.
    /// </summary>
    public string TableName { get; private set; } = null!;

    /// <summary>
    /// Gets the unique identifier of the record that was modified.
    /// </summary>
    public Guid RecordId { get; private set; }

    /// <summary>
    /// Gets the type of operation performed (e.g., Insert, Update, Delete).
    /// </summary>
    public AuditOperation Operation { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user who performed the operation, if known.
    /// </summary>
    public Guid? UserId { get; private set; }

    /// <summary>
    /// Gets the details of the changes, if any.
    /// </summary>
    public string? Changes { get; private set; }

    /// <summary>
    /// Gets the date and time when the audit log entry was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Creates a new audit log entry for tracking changes in the database.
    /// </summary>
    /// <param name="tableName">The name of the table where the change occurred.</param>
    /// <param name="recordId">The unique identifier of the record that was modified.</param>
    /// <param name="operation">The type of operation performed (e.g., Insert, Update, Delete).</param>
    /// <param name="userId">The unique identifier of the user who performed the operation, if known. Can be null.</param>
    /// <param name="changes">Details of the changes, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="AuditLog"/> with the specified parameters.</returns>
    public static AuditLog Create(
        string tableName,
        Guid recordId,
        AuditOperation operation,
        Guid? userId,
        string? changes)
    {
        return new AuditLog(
            tableName,
            recordId,
            operation,
            userId,
            changes);
    }
}

// <copyright file="CommentStatus.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Domain.Enums;

/// <summary>
/// Represents the moderation status of a comment.
/// </summary>
public enum CommentStatus
{
    /// <summary>
    /// The comment is pending review.
    /// </summary>
    Pending,

    /// <summary>
    /// The comment has been approved and is visible.
    /// </summary>
    Approved,

    /// <summary>
    /// The comment has been rejected and is hidden or deleted.
    /// </summary>
    Rejected,
}

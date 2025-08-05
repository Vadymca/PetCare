// <copyright file="DonationTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.Aggregates;

using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using System;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="Donation"/> aggregate.
/// </summary>
public sealed class DonationTests
{
    /// <summary>
    /// Tests that Create initializes properties correctly with valid inputs.
    /// </summary>
    [Fact]
    public void Create_WithValidParameters_ShouldInitializeProperties()
    {
        // Arrange
        Guid? userId = Guid.NewGuid();
        decimal amount = 100m;
        Guid? shelterId = Guid.NewGuid();
        Guid paymentMethodId = Guid.NewGuid();
        DonationStatus status = DonationStatus.Pending;
        string transactionId = "TX123";
        string purpose = "Help animals";
        bool recurring = true;
        bool anonymous = true;
        DateTime donationDate = DateTime.UtcNow.AddDays(-1);
        string report = "Report details";

        // Act
        var donation = Donation.Create(
            userId,
            amount,
            shelterId,
            paymentMethodId,
            status,
            transactionId,
            purpose,
            recurring,
            anonymous,
            donationDate,
            report);

        // Assert
        Assert.Equal(userId, donation.UserId);
        Assert.Equal(amount, donation.Amount);
        Assert.Equal(shelterId, donation.ShelterId);
        Assert.Equal(paymentMethodId, donation.PaymentMethodId);
        Assert.Equal(status, donation.Status);
        Assert.Equal(transactionId, donation.TransactionId);
        Assert.Equal(purpose, donation.Purpose);
        Assert.Equal(recurring, donation.Recurring);
        Assert.Equal(anonymous, donation.Anonymous);
        Assert.Equal(donationDate, donation.DonationDate);
        Assert.Equal(report, donation.Report);
        Assert.True((DateTime.UtcNow - donation.CreatedAt).TotalSeconds < 5);
        Assert.True((DateTime.UtcNow - donation.UpdatedAt).TotalSeconds < 5);
    }

    /// <summary>
    /// Tests that Create throws an exception when amount is zero or negative.
    /// </summary>
    /// <param name="invalidAmount">The invalid amount value to test (zero or negative).</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Create_WithNonPositiveAmount_ShouldThrowArgumentException(decimal invalidAmount)
    {
        // Arrange
        Guid? userId = Guid.NewGuid();
        Guid? shelterId = Guid.NewGuid();
        Guid paymentMethodId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            Donation.Create(
                userId,
                invalidAmount,
                shelterId,
                paymentMethodId,
                DonationStatus.Pending));

        Assert.Contains("Сума повинна бути більшою за 0.", ex.Message);
    }

    /// <summary>
    /// Tests UpdateReport updates the Report property and the UpdatedAt timestamp.
    /// </summary>
    [Fact]
    public void UpdateReport_ShouldUpdateReportAndTimestamp()
    {
        // Arrange
        var donation = CreateValidDonation();
        string newReport = "Updated report";

        var oldUpdatedAt = donation.UpdatedAt;

        // Act
        donation.UpdateReport(newReport);

        // Assert
        Assert.Equal(newReport, donation.Report);
        Assert.True(donation.UpdatedAt > oldUpdatedAt);
    }

    /// <summary>
    /// Tests MarkAsCompleted sets status to Completed and optionally updates TransactionId.
    /// </summary>
    [Fact]
    public void MarkAsCompleted_WithTransactionId_ShouldUpdateStatusAndTransactionId()
    {
        // Arrange
        var donation = CreateValidDonation();
        string newTransactionId = "TX999";

        var oldUpdatedAt = donation.UpdatedAt;

        // Act
        donation.MarkAsCompleted(newTransactionId);

        // Assert
        Assert.Equal(DonationStatus.Completed, donation.Status);
        Assert.Equal(newTransactionId, donation.TransactionId);
        Assert.True(donation.UpdatedAt > oldUpdatedAt);
    }

    /// <summary>
    /// Tests MarkAsCompleted without transaction id updates status but keeps existing TransactionId.
    /// </summary>
    [Fact]
    public void MarkAsCompleted_WithoutTransactionId_ShouldUpdateStatusOnly()
    {
        // Arrange
        var donation = CreateValidDonation();
        var oldTransactionId = donation.TransactionId;
        var oldUpdatedAt = donation.UpdatedAt;

        // Act
        donation.MarkAsCompleted(null);

        // Assert
        Assert.Equal(DonationStatus.Completed, donation.Status);
        Assert.Equal(oldTransactionId, donation.TransactionId);
        Assert.True(donation.UpdatedAt > oldUpdatedAt);
    }

    /// <summary>
    /// Tests MarkAsFailed sets status to Failed and updates UpdatedAt.
    /// </summary>
    [Fact]
    public void MarkAsFailed_ShouldSetStatusToFailedAndUpdateTimestamp()
    {
        // Arrange
        var donation = CreateValidDonation();
        var oldUpdatedAt = donation.UpdatedAt;

        // Act
        donation.MarkAsFailed();

        // Assert
        Assert.Equal(DonationStatus.Failed, donation.Status);
        Assert.True(donation.UpdatedAt > oldUpdatedAt);
    }

    /// <summary>
    /// Tests SetStatus updates the status and UpdatedAt timestamp.
    /// </summary>
    [Fact]
    public void SetStatus_ShouldUpdateStatusAndTimestamp()
    {
        // Arrange
        var donation = CreateValidDonation();
        var newStatus = DonationStatus.Completed;
        var oldUpdatedAt = donation.UpdatedAt;

        // Act
        donation.SetStatus(newStatus);

        // Assert
        Assert.Equal(newStatus, donation.Status);
        Assert.True(donation.UpdatedAt > oldUpdatedAt);
    }

    private static Donation CreateValidDonation()
    {
        return Donation.Create(
            Guid.NewGuid(),
            100m,
            Guid.NewGuid(),
            Guid.NewGuid(),
            DonationStatus.Pending,
            transactionId: "TX123",
            purpose: "Test donation",
            recurring: false,
            anonymous: false,
            donationDate: DateTime.UtcNow,
            report: "Initial report");
    }
}

// <copyright file="EventTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.Aggregates;
using FluentAssertions;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using System;
using Xunit;

/// <summary>
/// Tests for the <see cref="Event"/> aggregate.
/// </summary>
public class EventTests
{
    /// <summary>
    /// Tests creating an event with valid parameters.
    /// </summary>
    [Fact]
    public void Create_Should_Create_Event_With_Valid_Parameters()
    {
        // Arrange
        var shelterId = Guid.NewGuid();
        var title = "Event Title";
        string? description = "Some description";
        var futureDate = DateTime.UtcNow.AddDays(1);
        var location = Coordinates.From(45.0, 90.0);
        string? address = "123 Main St";
        var type = EventType.Webinar;
        var status = EventStatus.Planned;

        // Act
        var ev = Event.Create(
            shelterId,
            title,
            description,
            futureDate,
            location,
            address,
            type,
            status);

        // Assert
        ev.Should().NotBeNull();
        ev.ShelterId.Should().Be(shelterId);
        ev.Title.Value.Should().Be(title);
        ev.Description.Should().Be(description);
        ev.EventDate.Should().Be(futureDate);
        ev.Location.Should().Be(location);
        ev.Address.Should().NotBeNull();
        ev.Address!.Value.Should().Be(address);
        ev.Type.Should().Be(type);
        ev.Status.Should().Be(status);
        ev.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        ev.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Tests that creating an event with a past date throws an exception.
    /// </summary>
    [Fact]
    public void Create_Should_Throw_When_EventDate_In_Past()
    {
        // Arrange
        var pastDate = DateTime.UtcNow.AddMinutes(-1);

        // Act
        Action act = () => Event.Create(
            null,
            "Title",
            null,
            pastDate,
            null,
            null,
            EventType.AdoptionDay,
            EventStatus.Planned);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Дата події повинна бути в майбутньому*");
    }

    /// <summary>
    /// Tests updating the event's properties.
    /// </summary>
    [Fact]
    public void Update_Should_Update_Provided_Properties()
    {
        // Arrange
        var ev = Event.Create(
            null,
            "Initial",
            "Desc",
            DateTime.UtcNow.AddDays(1),
            null,
            null,
            EventType.Fundraiser,
            EventStatus.Planned);

        var newTitle = "Updated Title";
        var newDescription = "Updated Desc";
        var newDate = DateTime.UtcNow.AddDays(2);
        var newLocation = Coordinates.From(50, 50);
        var newAddress = "New Address";
        var newStatus = EventStatus.Cancelled;

        // Act
        ev.Update(newTitle, newDescription, newDate, newLocation, newAddress, newStatus);

        // Assert
        ev.Title.Value.Should().Be(newTitle);
        ev.Description.Should().Be(newDescription);
        ev.EventDate.Should().Be(newDate);
        ev.Location.Should().Be(newLocation);
        ev.Address.Should().NotBeNull();
        ev.Address!.Value.Should().Be(newAddress);
        ev.Status.Should().Be(newStatus);
    }

    /// <summary>
    /// Tests setting status to Cancelled if event is not already cancelled.
    /// </summary>
    [Fact]
    public void Cancel_Should_Set_Status_To_Cancelled_If_Not_Already()
    {
        // Arrange
        var ev = Event.Create(
            null,
            "Title",
            null,
            DateTime.UtcNow.AddDays(1),
            null,
            null,
            EventType.VolunteerTraining,
            EventStatus.Planned);

        // Act
        ev.Cancel();

        // Assert
        ev.Status.Should().Be(EventStatus.Cancelled);
    }

    /// <summary>
    /// Tests that cancelling an already cancelled event throws.
    /// </summary>
    [Fact]
    public void Cancel_Should_Throw_If_Already_Cancelled()
    {
        // Arrange
        var ev = Event.Create(
            null,
            "Title",
            null,
            DateTime.UtcNow.AddDays(1),
            null,
            null,
            EventType.VolunteerTraining,
            EventStatus.Cancelled);

        // Act
        Action act = () => ev.Cancel();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Подія вже скасована.");
    }

    /// <summary>
    /// Tests setting status to Completed if event is not already completed.
    /// </summary>
    [Fact]
    public void Complete_Should_Set_Status_To_Completed_If_Not_Already()
    {
        // Arrange
        var ev = Event.Create(
            null,
            "Title",
            null,
            DateTime.UtcNow.AddDays(1),
            null,
            null,
            EventType.VolunteerTraining,
            EventStatus.Planned);

        // Act
        ev.Complete();

        // Assert
        ev.Status.Should().Be(EventStatus.Completed);
    }

    /// <summary>
    /// Tests that completing an already completed event throws.
    /// </summary>
    [Fact]
    public void Complete_Should_Throw_If_Already_Completed()
    {
        // Arrange
        var ev = Event.Create(
            null,
            "Title",
            null,
            DateTime.UtcNow.AddDays(1),
            null,
            null,
            EventType.VolunteerTraining,
            EventStatus.Completed);

        // Act
        Action act = () => ev.Complete();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Подія вже завершена.");
    }

    /// <summary>
    /// Tests postponing event updates EventDate and Status.
    /// </summary>
    [Fact]
    public void Postpone_Should_Update_EventDate_And_Status()
    {
        // Arrange
        var ev = Event.Create(
            null,
            "Title",
            null,
            DateTime.UtcNow.AddDays(1),
            null,
            null,
            EventType.VolunteerTraining,
            EventStatus.Planned);

        var newDate = DateTime.UtcNow.AddDays(3);

        // Act
        ev.Postpone(newDate);

        // Assert
        ev.EventDate.Should().Be(newDate);
        ev.Status.Should().Be(EventStatus.Planned);
    }

    /// <summary>
    /// Tests that postponing event to past date throws.
    /// </summary>
    [Fact]
    public void Postpone_Should_Throw_If_NewDate_In_Past()
    {
        // Arrange
        var ev = Event.Create(
            null,
            "Title",
            null,
            DateTime.UtcNow.AddDays(1),
            null,
            null,
            EventType.VolunteerTraining,
            EventStatus.Planned);

        var pastDate = DateTime.UtcNow.AddMinutes(-5);

        // Act
        Action act = () => ev.Postpone(pastDate);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Нова дата повинна бути в майбутньому*");
    }

    /// <summary>
    /// Tests that postponing completed or cancelled event throws.
    /// </summary>
    /// <param name="status">Event status.</param>
    [Theory]
    [InlineData(EventStatus.Completed)]
    [InlineData(EventStatus.Cancelled)]
    public void Postpone_Should_Throw_If_Event_Completed_Or_Cancelled(EventStatus status)
    {
        // Arrange
        var ev = Event.Create(
            null,
            "Title",
            null,
            DateTime.UtcNow.AddDays(1),
            null,
            null,
            EventType.VolunteerTraining,
            status);

        var newDate = DateTime.UtcNow.AddDays(5);

        // Act
        Action act = () => ev.Postpone(newDate);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Не можна перенести завершену або скасовану подію.");
    }

    /// <summary>
    /// Tests updating coordinates changes Location and UpdatedAt.
    /// </summary>
    [Fact]
    public void UpdateCoordinates_Should_Change_Location_And_UpdatedAt()
    {
        // Arrange
        var ev = Event.Create(
            null,
            "Title",
            null,
            null,
            null,
            null,
            EventType.VolunteerTraining,
            EventStatus.Planned);

        var coordinates = Coordinates.From(10, 20);

        var oldUpdatedAt = ev.UpdatedAt;

        // Act
        ev.UpdateCoordinates(coordinates);

        // Assert
        ev.Location.Should().Be(coordinates);
        ev.UpdatedAt.Should().BeAfter(oldUpdatedAt);
    }

    /// <summary>
    /// Tests updating address changes Address and UpdatedAt.
    /// </summary>
    [Fact]
    public void UpdateAddress_Should_Change_Address_And_UpdatedAt()
    {
        // Arrange
        var ev = Event.Create(
            null,
            "Title",
            null,
            null,
            null,
            null,
            EventType.VolunteerTraining,
            EventStatus.Planned);

        var addressStr = "New Address 123";

        var oldUpdatedAt = ev.UpdatedAt;

        // Act
        ev.UpdateAddress(addressStr);

        // Assert
        ev.Address.Should().NotBeNull();
        ev.Address!.Value.Should().Be(addressStr);
        ev.UpdatedAt.Should().BeAfter(oldUpdatedAt);
    }

    /// <summary>
    /// Tests assigning shelter id updates ShelterId and UpdatedAt.
    /// </summary>
    [Fact]
    public void AssignToShelter_Should_Set_ShelterId_And_UpdatedAt()
    {
        // Arrange
        var ev = Event.Create(
            null,
            "Title",
            null,
            null,
            null,
            null,
            EventType.VolunteerTraining,
            EventStatus.Planned);

        var shelterId = Guid.NewGuid();

        var oldUpdatedAt = ev.UpdatedAt;

        // Act
        ev.AssignToShelter(shelterId);

        // Assert
        ev.ShelterId.Should().Be(shelterId);
        ev.UpdatedAt.Should().BeAfter(oldUpdatedAt);
    }
}
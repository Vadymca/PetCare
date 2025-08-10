namespace PetCare.Tests.Domain.Aggregates;

using FluentAssertions;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.Events;
using Xunit;

/// <summary>
/// Unit tests for <see cref="AdoptionApplication"/> aggregate.
/// </summary>
public class AdoptionApplicationTests
{
    /// <summary>
    /// Tests that creating an adoption application with valid parameters
    /// sets all properties correctly and adds the created domain event.
    /// </summary>
    [Fact]
    public void Create_WithValidParameters_ShouldCreateSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var animalId = Guid.NewGuid();
        var comment = "I love animals";

        // Act
        var application = AdoptionApplication.Create(userId, animalId, comment);

        // Assert
        application.UserId.Should().Be(userId);
        application.AnimalId.Should().Be(animalId);
        application.Comment.Should().Be(comment);
        application.Status.Should().Be(AdoptionStatus.Pending);
        application.IsPending.Should().BeTrue();
        application.ApplicationDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        application.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        application.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        application.DomainEvents.Should().ContainSingle(e => e is AdoptionApplicationCreatedEvent);
    }

    /// <summary>
    /// Tests that creating an adoption application with empty <paramref name="userId"/> or <paramref name="animalId"/>
    /// throws <see cref="ArgumentException"/> with the correct parameter name.
    /// </summary>
    /// <param name="userIdStr">The user ID as a string.</param>
    /// <param name="animalIdStr">The animal ID as a string.</param>
    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000", "11111111-1111-1111-1111-111111111111")]
    [InlineData("11111111-1111-1111-1111-111111111111", "00000000-0000-0000-0000-000000000000")]
    [InlineData("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
    public void Create_WithEmptyGuid_ShouldThrowArgumentException(string userIdStr, string animalIdStr)
    {
        // Arrange
        var userId = Guid.Parse(userIdStr);
        var animalId = Guid.Parse(animalIdStr);

        // Act
        Action act = () => AdoptionApplication.Create(userId, animalId, "comment");

        // Assert
        act.Should().Throw<ArgumentException>()
            .Where(ex => ex.ParamName == (userId == Guid.Empty ? "userId" : "animalId"));
    }

    /// <summary>
    /// Tests that approving a pending application sets the status to Approved,
    /// sets the approving admin ID, updates the timestamp, and adds the approved event.
    /// </summary>
    [Fact]
    public void Approve_WhenPending_ShouldSetStatusApprovedAndAddEvent()
    {
        // Arrange
        var app = AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), null);
        var adminId = Guid.NewGuid();

        // Act
        app.Approve(adminId);

        // Assert
        app.Status.Should().Be(AdoptionStatus.Approved);
        app.ApprovedBy.Should().Be(adminId);
        app.IsApproved.Should().BeTrue();
        app.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        app.DomainEvents.Should().ContainSingle(e => e is AdoptionApplicationApprovedEvent);
    }

    /// <summary>
    /// Tests that approving an application which is not pending
    /// throws <see cref="InvalidOperationException"/> with the correct message.
    /// </summary>
    [Fact]
    public void Approve_WhenNotPending_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var app = AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), null);
        var adminId = Guid.NewGuid();
        app.Approve(adminId);

        // Act
        Action act = () => app.Approve(adminId);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Затверджуються лише ті заявки, які знаходяться на розгляді.");
    }

    /// <summary>
    /// Tests that rejecting a pending application sets the status to Rejected,
    /// sets the rejection reason, updates the timestamp, and adds the rejected event.
    /// </summary>
    [Fact]
    public void Reject_WhenPending_ShouldSetStatusRejectedAndAddEvent()
    {
        // Arrange
        var app = AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), null);
        var reason = "Not suitable";

        // Act
        app.Reject(reason);

        // Assert
        app.Status.Should().Be(AdoptionStatus.Rejected);
        app.RejectionReason.Should().Be(reason);
        app.IsRejected.Should().BeTrue();
        app.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        app.DomainEvents.Should().ContainSingle(e => e is AdoptionApplicationRejectedEvent);
    }

    /// <summary>
    /// Tests that rejecting an application which is not pending
    /// throws <see cref="InvalidOperationException"/> with the correct message.
    /// </summary>
    [Fact]
    public void Reject_WhenNotPending_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var app = AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), null);
        app.Reject("reason");

        // Act
        Action act = () => app.Reject("another reason");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Відхилити можна лише ті заявки, що перебувають на розгляді.");
    }

    /// <summary>
    /// Tests that adding valid administrative notes updates the notes,
    /// updates the timestamp, and adds the notes updated event.
    /// </summary>
    [Fact]
    public void AddAdminNotes_WithValidNotes_ShouldUpdateNotesAndAddEvent()
    {
        // Arrange
        var app = AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), null);
        var notes = "Admin note";

        // Act
        app.AddAdminNotes(notes);

        // Assert
        app.AdminNotes.Should().Be(notes);
        app.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        app.DomainEvents.Should().ContainSingle(e => e is AdoptionApplicationNotesUpdatedEvent);
    }

    /// <summary>
    /// Tests that adding null, empty, or whitespace administrative notes
    /// throws <see cref="ArgumentException"/> with the correct parameter name.
    /// </summary>
    /// <param name="invalidNotes">The invalid notes string.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void AddAdminNotes_WithNullOrWhitespace_ShouldThrowArgumentException(string invalidNotes)
    {
        // Arrange
        var app = AdoptionApplication.Create(Guid.NewGuid(), Guid.NewGuid(), null);

        // Act
        Action act = () => app.AddAdminNotes(invalidNotes);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Адміністративні нотатки не можуть бути порожніми.*")
            .Where(ex => ex.ParamName == "notes");
    }
}

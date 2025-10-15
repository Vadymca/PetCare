namespace PetCare.Tests.Infrastructure.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using PetCare.Domain.Abstractions.Events;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using PetCare.Infrastructure.Persistence;
using PetCare.Infrastructure.Services.Identity;

/// <summary>
/// Contains unit tests for the AuthorizationService class, verifying role-based authorization logic.
/// </summary>
/// <remarks>These tests use mocked dependencies and an in-memory database to isolate and validate the behavior of
/// the AuthorizationService. The tests focus on scenarios such as checking user roles and handling cases where users
/// are not found.</remarks>
public class AuthorizationServiceTests
{
    private readonly AuthorizationService service;
    private readonly Mock<UserManager<User>> userManagerMock;
    private readonly AppDbContext dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationServiceTests"/> class for unit testing the AuthorizationService.
    /// component.
    /// </summary>
    /// <remarks>This constructor sets up in-memory dependencies and mock objects required for isolated
    /// testing of authorization logic. It configures an in-memory database and mock implementations of user management
    /// and domain event dispatching to ensure tests do not depend on external resources.</remarks>
    public AuthorizationServiceTests()
    {
        var store = new Mock<IUserStore<User>>();
        this.userManagerMock = new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        // Mock dispatcher
        var dispatcherMock = new Mock<IDomainEventDispatcher>();

        this.dbContext = new AppDbContext(options, dispatcherMock.Object);
        this.service = new AuthorizationService(this.dbContext, this.userManagerMock.Object);
    }

    /// <summary>
    /// Verifies that HasRoleAsync returns true when the specified user is assigned the given role.
    /// </summary>
    /// <remarks>This unit test ensures that the HasRoleAsync method correctly identifies when a user
    /// possesses a specific role. It uses mocked dependencies to simulate the user and role assignment.</remarks>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task HasRoleAsync_ShouldReturnTrue_WhenUserHasRole()
    {
        var user = User.Create(
            email: "test@example.com",
            passwordHash: "hashedPassword",
            firstName: "Test",
            lastName: "User",
            phone: "+380501234567",
            role: UserRole.User,
            preferences: new Dictionary<string, string> { { "theme", "dark" } },
            points: 100,
            lastLogin: DateTime.UtcNow,
            profilePhoto: null,
            language: "uk");
        this.userManagerMock.Setup(x => x.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);
        this.userManagerMock.Setup(x => x.IsInRoleAsync(user, Role.Admin.ToString())).ReturnsAsync(true);

        var result = await this.service.HasRoleAsync(user.Id, Role.Admin, CancellationToken.None);

        Assert.True(result);
    }

    /// <summary>
    /// Verifies that HasRoleAsync returns false when the specified user cannot be found.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task HasRoleAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        this.userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var result = await this.service.HasRoleAsync(Guid.NewGuid(), Role.Admin, CancellationToken.None);

        Assert.False(result);
    }
}

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

public class AuthorizationServiceTests
{
    private readonly AuthorizationService service;
    private readonly Mock<UserManager<User>> userManagerMock;
    private readonly AppDbContext dbContext;

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

    [Fact]
    public async Task HasRoleAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        this.userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var result = await this.service.HasRoleAsync(Guid.NewGuid(), Role.Admin, CancellationToken.None);

        Assert.False(result);
    }
}

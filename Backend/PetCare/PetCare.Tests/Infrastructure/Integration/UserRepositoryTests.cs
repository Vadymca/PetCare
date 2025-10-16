namespace PetCare.Tests.Infrastructure.Integration;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;
using PetCare.Domain.Abstractions.Events;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using PetCare.Infrastructure.Persistence;
using PetCare.Infrastructure.Persistence.Repositories;
using Testcontainers.PostgreSql;

/// <summary>
/// Integration tests for <see cref="UserRepository"/>.
/// Uses Testcontainers with PostgreSQL for full database verification.
/// </summary>
public sealed class UserRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer postgres;
    private AppDbContext context = null!;
    private UserRepository repository = null!;

    /// <summary>
    /// Test user data for seeding.
    /// </summary>
    private User testUser1 = null!;
    private User testUser2 = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepositoryTests"/> class.
    /// Configures PostgreSQL test container.
    /// </summary>
    public UserRepositoryTests()
    {
        this.postgres = new PostgreSqlBuilder()
            .WithImage("postgis/postgis:16-3.4")
            .WithDatabase("petcare_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    /// <summary>
    /// Sets up database and repository before tests.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Obsolete]
    public async Task InitializeAsync()
    {
        // Enable JSON support in Npgsql
        NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

        await this.postgres.StartAsync();

        var dispatcherMock = new Mock<IDomainEventDispatcher>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(this.postgres.GetConnectionString(), o =>
            {
                o.UseNetTopologySuite();
                o.MapEnum<UserRole>();
            })
            .Options;

        this.context = new AppDbContext(options, dispatcherMock.Object);
        await this.context.Database.EnsureCreatedAsync();

        this.repository = new UserRepository(this.context);

        await this.SeedTestDataAsync();
    }

    /// <summary>
    /// Disposes database context and stops PostgreSQL container after tests.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DisposeAsync()
    {
        await this.context.DisposeAsync();
        await this.postgres.StopAsync();
    }

    /// <summary>
    /// Tests that GetByRoleAsync returns all users with a given role.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetByRoleAsync_ShouldReturnUsersWithSpecifiedRole()
    {
        // Act
        var users = await this.repository.GetByRoleAsync(UserRole.User);

        // Assert
        Assert.Single(users);
        Assert.Equal(this.testUser1.Id, users.First().Id);
    }

    /// <summary>
    /// Tests that GetUsersByShelterSubscriptionAsync returns users subscribed to a given shelter.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetUsersByShelterSubscriptionAsync_ShouldReturnSubscribedUsers()
    {
        // Arrange: створюємо Shelter
        var shelter = Shelter.Create(
            name: "Test Shelter",
            address: "123 Test Street",
            coordinates: Coordinates.From(50.45, 30.52),
            contactPhone: "+380501112233",
            contactEmail: "shelter@test.com",
            description: "Test description",
            capacity: 100,
            currentOccupancy: 10,
            photos: new List<string>(),
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: this.testUser2.Id);

        await this.context.Shelters.AddAsync(shelter);
        await this.context.SaveChangesAsync();

        var subscription = ShelterSubscription.Create(this.testUser1.Id, shelter.Id);
        await this.context.AddAsync(subscription);
        await this.context.SaveChangesAsync();

        // Act
        var subscribedUsers = await this.repository.GetUsersByShelterSubscriptionAsync(shelter.Id);

        // Assert
        Assert.Single(subscribedUsers);
        Assert.Equal(this.testUser1.Id, subscribedUsers.First().Id);
    }

    /// <summary>
    /// Seeds test users for repository tests.
    /// </summary>
    private async Task SeedTestDataAsync()
    {
        this.testUser1 = User.Create(
            email: "user1@example.com",
            passwordHash: "hash1",
            firstName: "User",
            lastName: "One",
            phone: "+380501234567",
            role: UserRole.User);

        this.testUser2 = User.Create(
            email: "admin@example.com",
            passwordHash: "hash2",
            firstName: "Admin",
            lastName: "Two",
            phone: "+380501234568",
            role: UserRole.Admin);

        await this.context.Users.AddRangeAsync(this.testUser1, this.testUser2);
        await this.context.SaveChangesAsync();
    }
}

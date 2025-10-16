namespace PetCare.Tests.Infrastructure.Integration;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;
using PetCare.Domain.Abstractions.Events;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;
using PetCare.Infrastructure.Persistence;
using PetCare.Infrastructure.Persistence.Repositories;
using Testcontainers.PostgreSql;

/// <summary>
/// Integration tests for <see cref="VolunteerTaskRepository"/>.
/// Uses Testcontainers with PostgreSQL for full database verification.
/// </summary>
public sealed class VolunteerTaskRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer postgres;
    private AppDbContext context = null!;
    private VolunteerTaskRepository repository = null!;
    private Shelter testShelter = null!;
    private User testUser1 = null!;
    private User testUser2 = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="VolunteerTaskRepositoryTests"/> class.
    /// Configures PostgreSQL test container.
    /// </summary>
    public VolunteerTaskRepositoryTests()
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
        NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
        await this.postgres.StartAsync();

        var dispatcherMock = new Mock<IDomainEventDispatcher>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseNpgsql(this.postgres.GetConnectionString(), o =>
           {
               o.UseNetTopologySuite();
               o.MapEnum<UserRole>();
               o.MapEnum<VolunteerTaskStatus>();
           })
            .Options;

        this.context = new AppDbContext(options, dispatcherMock.Object);
        await this.context.Database.EnsureCreatedAsync();

        this.repository = new VolunteerTaskRepository(this.context);

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
    /// Tests that GetByShelterIdAsync returns all tasks for a shelter.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetByShelterIdAsync_ShouldReturnTasksForShelter()
    {
        var tasks = await this.repository.GetByShelterIdAsync(this.testShelter.Id);

        Assert.Equal(3, tasks.Count);
        Assert.All(tasks, t => Assert.Equal(this.testShelter.Id, t.ShelterId));
    }

    /// <summary>
    /// Tests that GetByDateAsync returns tasks for a specific date.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task GetByDateAsync_ShouldReturnTasksForSpecificDate()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var tasks = await this.repository.GetByDateAsync(today);

        Assert.Equal(2, tasks.Count); // Task 1 і Task 3
        Assert.All(tasks, t => Assert.Equal(today, t.Date));
    }

    /// <summary>
    /// Creates a volunteer task for testing.
    /// </summary>
    /// <param name="title">Task title.</param>
    /// <param name="date">Task date.</param>
    /// <returns>A new <see cref="VolunteerTask"/> instance.</returns>
    private VolunteerTask CreateTestTask(string title, DateOnly date)
    {
        return VolunteerTask.Create(
            shelterId: this.testShelter.Id,
            title: title,
            description: null,
            date: date,
            duration: 2,
            requiredVolunteers: 3,
            status: VolunteerTaskStatus.Open,
            pointsReward: 50,
            location: null,
            skillsRequired: null);
    }

    /// <summary>
    /// Seeds users, shelter, and volunteer tasks for repository tests.
    /// </summary>
    private async Task SeedTestDataAsync()
    {
        // 1. Create test users
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

        // 2. Create test shelter
        this.testShelter = Shelter.Create(
            name: "Test Shelter",
            address: "123 Test Street",
            coordinates: Coordinates.From(50.45, 30.52),
            contactPhone: "+380501112233",
            contactEmail: "shelter@test.com",
            description: "Test description",
            capacity: 50,
            currentOccupancy: 5,
            photos: new List<string>(),
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: this.testUser2.Id);

        await this.context.Shelters.AddAsync(this.testShelter);
        await this.context.SaveChangesAsync();

        // 3. Add volunteer tasks
        var tasks = new List<VolunteerTask>
        {
            this.CreateTestTask("Task 1", DateOnly.FromDateTime(DateTime.UtcNow)),
            this.CreateTestTask("Task 2", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))),
            this.CreateTestTask("Task 3", DateOnly.FromDateTime(DateTime.UtcNow)),
        };

        await this.context.VolunteerTasks.AddRangeAsync(tasks);
        await this.context.SaveChangesAsync();
    }
}
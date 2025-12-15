namespace PetCare.Tests.Infrastructure.Integration;

using System.Collections.Generic;
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
using Xunit;

/// <summary>
/// Integration tests for <see cref="AdoptionApplicationRepository"/>.
/// </summary>
public sealed class AdoptionApplicationRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer postgres;
    private AppDbContext context = null!;
    private AdoptionApplicationRepository repository = null!;

    private User testUser = null!;
    private Animal testAnimal = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdoptionApplicationRepositoryTests"/> class.
    /// </summary>
    public AdoptionApplicationRepositoryTests()
    {
        this.postgres = new PostgreSqlBuilder()
            .WithImage("postgis/postgis:16-3.4")
            .WithDatabase("petcare_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    /// <summary>
    /// Sets up the database and seeds initial data before each test run.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Obsolete]
    public async Task InitializeAsync()
    {
        NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
        await this.postgres.StartAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(this.postgres.GetConnectionString(), o =>
            {
                o.UseNetTopologySuite();
                o.MapEnum<UserRole>();
                o.MapEnum<AnimalGender>();
                o.MapEnum<AnimalStatus>();
                o.MapEnum<AdoptionStatus>();
            })
            .Options;

        var dispatcherMock = new Mock<IDomainEventDispatcher>();
        this.context = new AppDbContext(options, dispatcherMock.Object);
        await this.context.Database.EnsureCreatedAsync();

        this.repository = new AdoptionApplicationRepository(this.context);

        // Seed test user, shelter, species, breed, animal
        await this.SeedTestDataAsync();
    }

    /// <summary>
    /// Disposes the database context and stops the Postgres container.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DisposeAsync()
    {
        await this.context.DisposeAsync();
        await this.postgres.StopAsync();
    }

    /// <summary>
    /// Tests that an adoption application can be added and persisted.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task AddAsync_ShouldPersistApplication()
    {
        var app = AdoptionApplication.Create(this.testUser.Id, this.testAnimal.Id, "I want to adopt");
        await this.repository.AddAsync(app);
        await this.context.SaveChangesAsync();

        var fromDb = await this.repository.GetByIdAsync(app.Id);
        Assert.NotNull(fromDb);
        Assert.Equal(this.testUser.Id, fromDb!.UserId);
        Assert.Equal(this.testAnimal.Id, fromDb.AnimalId);
        Assert.Equal("I want to adopt", fromDb.Comment);
        Assert.Equal(AdoptionStatus.Pending, fromDb.Status);
    }

    private async Task SeedTestDataAsync()
    {
        // 1. User
        this.testUser = User.Create(
            email: "user@example.com",
            passwordHash: "hashed_password_here",
            firstName: "User",
            lastName: "Name",
            phone: "+380501234567",
            role: UserRole.User);
        await this.context.Users.AddAsync(this.testUser);
        await this.context.SaveChangesAsync();

        // 2. Shelter
        var shelter = Shelter.Create(
            name: "ShelterName",
            address: "Street, City, Country, 12345",
            coordinates: Coordinates.From(50.45, 30.52),
            contactPhone: "+380501234567",
            contactEmail: "shelter@example.com",
            description: "Safe place for animals",
            capacity: 50,
            currentOccupancy: 10,
            photos: new List<string>(),
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: this.testUser.Id);
        await this.context.Shelters.AddAsync(shelter);
        await this.context.SaveChangesAsync();

        // 3. Species
        var species = Specie.Create("Dog");
        await this.context.Species.AddAsync(species);
        await this.context.SaveChangesAsync();

        // 4. Breed
        var breed = Breed.Create(species.Id, "Dog", "Common domestic dog");
        await this.context.Breeds.AddAsync(breed);
        await this.context.SaveChangesAsync();

        // 5. Animal
        this.testAnimal = Animal.Create(
            userId: this.testUser.Id,
            name: "Doggy",
            breedId: breed.Id,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthConditions: new List<string>(),
            specialNeeds: new List<string>(),
            temperaments: new List<AnimalTemperament>(),
            size: AnimalSize.Medium,
            photos: new List<string>(),
            videos: new List<string>(),
            shelterId: shelter.Id,
            status: AnimalStatus.Available,
            careCost: AnimalCareCost.SixHundred,
            adoptionRequirements: null,
            microchipId: null,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            isUnderCare: false,
            haveDocuments: false);
        await this.context.Animals.AddAsync(this.testAnimal);
        await this.context.SaveChangesAsync();
    }
}
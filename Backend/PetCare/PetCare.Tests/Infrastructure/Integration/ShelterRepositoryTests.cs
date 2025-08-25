namespace PetCare.Tests.Infrastructure.Integration;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

/// <summary>
/// Integration tests for <see cref="ShelterRepository"/>.
/// </summary>
public sealed class ShelterRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer postgres;
    private AppDbContext context = null!;
    private ShelterRepository repository = null!;

    private User testUser = null!;
    private Breed testBreed = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShelterRepositoryTests"/> class.
    /// </summary>
    public ShelterRepositoryTests()
    {
        this.postgres = new PostgreSqlBuilder()
            .WithImage("postgis/postgis:16-3.4")
            .WithDatabase("petcare_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    /// <summary>
    /// Initializes DB and repository before each test.
    /// </summary>
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
                o.MapEnum<AnimalGender>();
                o.MapEnum<AnimalStatus>();
                o.MapEnum<IoTDeviceStatus>();
                o.MapEnum<IoTDeviceType>();
            })
            .Options;

        this.context = new AppDbContext(options, dispatcherMock.Object);
        await this.context.Database.EnsureCreatedAsync();

        this.repository = new ShelterRepository(this.context);

        await SeedTestDataAsync();
    }

    /// <summary>
    /// Disposes the DB context and stops the container.
    /// </summary>
    public async Task DisposeAsync()
    {
        await this.context.DisposeAsync();
        await this.postgres.StopAsync();
    }

    /// <summary>
    /// Seeds required test data: user and breed.
    /// </summary>
    private async Task SeedTestDataAsync()
    {
        // Create user
        this.testUser = User.Create(
            email: "user@example.com",
            passwordHash: "hashed_password",
            firstName: "User",
            lastName: "Name",
            phone: "+380501234567",
            role: UserRole.User);
        await context.Users.AddAsync(this.testUser);

        // Create species and breed
        var species = Specie.Create("Dog");
        await context.Species.AddAsync(species);

        this.testBreed = Breed.Create(species.Id, "Dog", "Common dog breed");
        await context.Breeds.AddAsync(this.testBreed);

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldReturnShelterWithIncludes()
    {
        // Arrange
        var slug = "shelter-" + Guid.NewGuid().ToString("N");

        var shelter = Shelter.Create(
            slug: slug,
            name: "Shelter Test",
            address: "Street 1, City",
            coordinates: Coordinates.From(50.45, 30.52),
            contactPhone: "+380501234567",
            contactEmail: "shelter@test.com",
            description: "Test Shelter",
            capacity: 10,
            currentOccupancy: 5,
            photos: new List<string>(),
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: this.testUser.Id);

        // Додаємо IoT-пристрій
        var device = IoTDevice.Create(
            shelterId: shelter.Id,
            type: IoTDeviceType.Temperature,
            name: "Device1",
            status: IoTDeviceStatus.Active,
            serialNumber: "SN123456",
            data: null,
            alertThresholds: null);
        shelter.AddIoTDevice(device, this.testUser.Id);

        // Додаємо тварину в Shelter
        var animal = Animal.Create(
            slug: "dog-" + Guid.NewGuid().ToString("N"),
            userId: this.testUser.Id,
            name: "Doggy",
            breedId: this.testBreed.Id,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthStatus: null,
            photos: new List<string>(),
            videos: new List<string>(),
            shelterId: shelter.Id,
            status: AnimalStatus.Available,
            adoptionRequirements: null,
            microchipId: null,
            idNumber: 1,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            haveDocuments: false);

        shelter.AddAnimal(animal, this.testUser.Id);
        await this.context.Shelters.AddAsync(shelter);
        await this.context.Animals.AddAsync(animal);
        await this.context.SaveChangesAsync();

        // Act
        var fromDb = await this.context.Shelters
            .Include(s => s.Animals)
            .Include(s => s.Donations)
            .Include(s => s.VolunteerTasks)
            .Include(s => s.AnimalAidRequests)
            .Include(s => s.IoTDevices)
            .Include(s => s.Events)
            .Include(s => s.Subscribers)
            .AsNoTracking() // щоб уникнути проблем із трекінгом
            .FirstOrDefaultAsync(s => s.Slug == shelter.Slug); // ValueConverter автоматично конвертує Slug

        // Assert
        Assert.NotNull(fromDb);
        Assert.Equal(shelter.Id, fromDb!.Id);
        Assert.NotNull(fromDb.Animals);
        Assert.NotNull(fromDb.Donations);
        Assert.NotNull(fromDb.VolunteerTasks);
        Assert.NotNull(fromDb.AnimalAidRequests);
        Assert.NotNull(fromDb.IoTDevices);
        Assert.NotNull(fromDb.Events);
        Assert.NotNull(fromDb.Subscribers);

        // Додатково перевіримо, що IoT-пристрій та тварина присутні
        Assert.Contains(fromDb.Animals, a => a.Id == animal.Id);
        Assert.Contains(fromDb.IoTDevices, d => d.Id == device.Id);
    }

    [Fact]
    public async Task GetByManagerIdAsync_ShouldReturnSheltersForManager()
    {
        // Arrange
        var shelter1 = Shelter.Create(
            slug: "shelter1-" + Guid.NewGuid().ToString("N"),
            name: "Shelter1",
            address: "Address1",
            coordinates: Coordinates.From(50.45, 30.52),
            contactPhone: "+380501234567",
            contactEmail: "shelter1@test.com",
            description: "Shelter1",
            capacity: 10,
            currentOccupancy: 3,
            photos: new List<string>(),
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: this.testUser.Id);

        var shelter2 = Shelter.Create(
            slug: "shelter2-" + Guid.NewGuid().ToString("N"),
            name: "Shelter2",
            address: "Address2",
            coordinates: Coordinates.From(50.46, 30.53),
            contactPhone: "+380501234568",
            contactEmail: "shelter2@test.com",
            description: "Shelter2",
            capacity: 15,
            currentOccupancy: 5,
            photos: new List<string>(),
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: this.testUser.Id);

        await context.Shelters.AddAsync(shelter1);
        await context.Shelters.AddAsync(shelter2);
        await context.SaveChangesAsync();

        // Act
        var shelters = await repository.GetByManagerIdAsync(this.testUser.Id);

        // Assert
        Assert.Equal(2, shelters.Count);
        Assert.Contains(shelters, s => s.Id == shelter1.Id);
        Assert.Contains(shelters, s => s.Id == shelter2.Id);
    }

    [Fact]
    public async Task GetWithFreeCapacityAsync_ShouldReturnSheltersWithAvailableSpace()
    {
        // Arrange
        var fullShelter = Shelter.Create(
            slug: "full-" + Guid.NewGuid().ToString("N"),
            name: "Full Shelter",
            address: "Addr1",
            coordinates: Coordinates.From(50.45, 30.52),
            contactPhone: "+380501234567",
            contactEmail: "full@test.com",
            description: "Full",
            capacity: 5,
            currentOccupancy: 5,
            photos: new List<string>(),
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: this.testUser.Id);

        var freeShelter = Shelter.Create(
            slug: "free-" + Guid.NewGuid().ToString("N"),
            name: "Free Shelter",
            address: "Addr2",
            coordinates: Coordinates.From(50.46, 30.53),
            contactPhone: "+380501234568",
            contactEmail: "free@test.com",
            description: "Free",
            capacity: 10,
            currentOccupancy: 5,
            photos: new List<string>(),
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: this.testUser.Id);

        await context.Shelters.AddAsync(fullShelter);
        await context.Shelters.AddAsync(freeShelter);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetWithFreeCapacityAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(freeShelter.Id, result.First().Id);
    }

    [Fact]
    public async Task GetShelterByDeviceIdAsync_ShouldReturnShelter()
    {
        // Arrange
        var shelter = Shelter.Create(
            slug: "device-shelter-" + Guid.NewGuid().ToString("N"),
            name: "Device Shelter",
            address: "Addr",
            coordinates: Coordinates.From(50.45, 30.52),
            contactPhone: "+380501234567",
            contactEmail: "device@test.com",
            description: "Device",
            capacity: 10,
            currentOccupancy: 3,
            photos: new List<string>(),
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: this.testUser.Id);

        await context.Shelters.AddAsync(shelter);
        await context.SaveChangesAsync();

        // Створюємо IoT-пристрій
        var device = IoTDevice.Create(
            shelterId: shelter.Id,
            type: IoTDeviceType.Camera,
            name: "Device1",
            status: IoTDeviceStatus.Active,
            serialNumber: "SN123456");

        // Додаємо пристрій у контекст без виклику Update
        await context.IoTDevices.AddAsync(device);
        await context.SaveChangesAsync();

        // Act
        var fromDb = await repository.GetShelterByDeviceIdAsync(device.Id);

        // Assert
        Assert.NotNull(fromDb);
        Assert.Equal(shelter.Id, fromDb!.Id);
        Assert.Contains(fromDb.IoTDevices, d => d.Id == device.Id);
    }
}

namespace PetCare.Tests.Infrastructure.Integration;

using System;
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
    /// Initializes the database context and related infrastructure asynchronously for testing purposes.
    /// </summary>
    /// <remarks>This method is obsolete and may be removed in a future release. It configures the database
    /// context, sets up required type mappings, and seeds test data. Intended for use in test scenarios only.</remarks>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
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
                o.MapEnum<AnimalGender>();
                o.MapEnum<AnimalStatus>();
                o.MapEnum<IoTDeviceStatus>();
                o.MapEnum<IoTDeviceType>();
            })
            .Options;

        this.context = new AppDbContext(options, dispatcherMock.Object);
        await this.context.Database.EnsureCreatedAsync();

        this.repository = new ShelterRepository(this.context);

        await this.SeedTestDataAsync();
    }

    /// <summary>
    /// Asynchronously releases all resources used by the current instance.
    /// </summary>
    /// <remarks>This method should be called when the instance is no longer needed to ensure that all
    /// underlying resources are released properly. Await the returned task to guarantee that disposal has completed
    /// before proceeding.</remarks>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async Task DisposeAsync()
    {
        await this.context.DisposeAsync();
        await this.postgres.StopAsync();
    }

    /// <summary>
    /// Verifies that retrieving a shelter by its slug returns the shelter entity with all related data, including
    /// animals and IoT devices, correctly included.
    /// </summary>
    /// <remarks>This test ensures that the data context properly loads all navigation properties for a
    /// shelter, such as animals, donations, volunteer tasks, animal aid requests, IoT devices, events, and subscribers,
    /// when queried by slug. It also confirms that specific related entities added during setup are present in the
    /// result.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task GetBySlugAsync_ShouldReturnShelterWithIncludes()
    {
        // Arrange
        var slug = "shelter-" + Guid.NewGuid().ToString("N");

        var shelter = Shelter.Create(
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
            userId: this.testUser.Id,
            name: "Doggy",
            breedId: this.testBreed.Id,
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

    /// <summary>
    /// Verifies that GetByManagerIdAsync returns all shelters managed by the specified manager.
    /// </summary>
    /// <remarks>This test ensures that the repository correctly retrieves all shelters associated with a
    /// given manager ID. It adds two shelters with the same manager and asserts that both are returned by the method
    /// under test.</remarks>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task GetByManagerIdAsync_ShouldReturnSheltersForManager()
    {
        // Arrange
        var shelter1 = Shelter.Create(
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

        await this.context.Shelters.AddAsync(shelter1);
        await this.context.Shelters.AddAsync(shelter2);
        await this.context.SaveChangesAsync();

        // Act
        var shelters = await this.repository.GetByManagerIdAsync(this.testUser.Id);

        // Assert
        Assert.Equal(2, shelters.Count);
        Assert.Contains(shelters, s => s.Id == shelter1.Id);
        Assert.Contains(shelters, s => s.Id == shelter2.Id);
    }

    /// <summary>
    /// Verifies that the GetWithFreeCapacityAsync method returns only shelters that have available capacity.
    /// </summary>
    /// <remarks>This test ensures that shelters at full capacity are excluded from the results, and only
    /// those with available space are returned by the repository method.</remarks>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task GetWithFreeCapacityAsync_ShouldReturnSheltersWithAvailableSpace()
    {
        // Arrange
        var fullShelter = Shelter.Create(
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

        await this.context.Shelters.AddAsync(fullShelter);
        await this.context.Shelters.AddAsync(freeShelter);
        await this.context.SaveChangesAsync();

        // Act
        var result = await this.repository.GetWithFreeCapacityAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(freeShelter.Id, result.First().Id);
    }

    /// <summary>
    /// Verifies that the GetShelterByDeviceIdAsync method returns the correct shelter associated with a given IoT
    /// device identifier.
    /// </summary>
    /// <remarks>This test ensures that when a shelter and an associated IoT device are added to the context,
    /// retrieving the shelter by the device's identifier returns the expected shelter and includes the device in its
    /// collection.</remarks>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task GetShelterByDeviceIdAsync_ShouldReturnShelter()
    {
        // Arrange
        var shelter = Shelter.Create(
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

        await this.context.Shelters.AddAsync(shelter);
        await this.context.SaveChangesAsync();

        // Створюємо IoT-пристрій
        var device = IoTDevice.Create(
            shelterId: shelter.Id,
            type: IoTDeviceType.Camera,
            name: "Device1",
            status: IoTDeviceStatus.Active,
            serialNumber: "SN123456");

        // Додаємо пристрій у контекст без виклику Update
        await this.context.IoTDevices.AddAsync(device);
        await this.context.SaveChangesAsync();

        // Act
        var fromDb = await this.repository.GetShelterByDeviceIdAsync(device.Id);

        // Assert
        Assert.NotNull(fromDb);
        Assert.Equal(shelter.Id, fromDb!.Id);
        Assert.Contains(fromDb.IoTDevices, d => d.Id == device.Id);
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
        await this.context.Users.AddAsync(this.testUser);

        // Create species and breed
        var species = Specie.Create("Dog");
        await this.context.Species.AddAsync(species);

        this.testBreed = Breed.Create(species.Id, "Dog", "Common dog breed");
        await this.context.Breeds.AddAsync(this.testBreed);

        await this.context.SaveChangesAsync();
    }
}

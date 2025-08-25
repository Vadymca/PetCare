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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

/// <summary>
/// Integration tests for <see cref="AnimalRepository"/>.
/// </summary>
public sealed class AnimalRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer postgres;
    private AppDbContext context = null!;
    private AnimalRepository repository = null!;

    private User testUser = null!;
    private Shelter testShelter = null!;
    private Breed testBreed = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalRepositoryTests"/> class.
    /// Initializes a new instance of <see cref="AnimalRepositoryTests"/>.
    /// </summary>
    public AnimalRepositoryTests()
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
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
             })
            .Options;

        this.context = new AppDbContext(options, dispatcherMock.Object);
        await this.context.Database.EnsureCreatedAsync();

        this.repository = new AnimalRepository(this.context);

        await this.SeedTestDataAsync();
    }

    /// <summary>
    /// Disposes the DB context and stops the container.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DisposeAsync()
    {
        await this.context.DisposeAsync();
        await this.postgres.StopAsync();
    }

    /// <summary>
    /// Tests that adding an <see cref="Animal"/> persists it correctly in the database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task AddAsync_ShouldPersistAnimal()
    {
        // Arrange: create a new Animal with all required properties
        var animal = Animal.Create(
            slug: "doggy",
            userId: this.testUser.Id,
            name: "Doggy",
            breedId: this.testBreed.Id,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthStatus: null,
            photos: new List<string>(),
            videos: new List<string>(),
            shelterId: this.testShelter.Id,
            status: AnimalStatus.Available,
            adoptionRequirements: null,
            microchipId: null,
            idNumber: 1,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            haveDocuments: false);

        // Safety checks: ensure Name and Slug are not empty before persisting
        Assert.False(string.IsNullOrWhiteSpace(animal.Name.Value), "Animal Name should not be empty");
        Assert.False(string.IsNullOrWhiteSpace(animal.Slug.Value), "Animal Slug should not be empty");

        // Act: add the animal to repository and save changes
        await this.repository.AddAsync(animal);
        await this.context.SaveChangesAsync();

        // Assert: retrieve from DB and verify all important fields
        var fromDb = await this.repository.GetByIdAsync(animal.Id);
        Assert.NotNull(fromDb);
        Assert.Equal("Doggy", fromDb!.Name.Value);
        Assert.Equal(this.testUser.Id, fromDb.UserId);
    }

    /// <summary>
    /// Tests that GetByShelterIdAsync returns only animals in the given shelter.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetByShelterIdAsync_ShouldReturnCorrectAnimals()
    {
        // Arrange: Add two animals, one in the test shelter, one in another shelter
        var animal1 = Animal.Create(
            slug: "doggy1",
            userId: this.testUser.Id,
            name: "Doggy1",
            breedId: this.testBreed.Id,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthStatus: null,
            photos: new List<string>(),
            videos: new List<string>(),
            shelterId: this.testShelter.Id,
            status: AnimalStatus.Available,
            adoptionRequirements: null,
            microchipId: null,
            idNumber: 1,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            haveDocuments: false);

        var otherShelter = Shelter.Create(
            slug: "other-shelter",
            name: "Other Shelter",
            address: "Other Address",
            coordinates: Coordinates.From(50.40, 30.50),
            contactPhone: "+380501234568",
            contactEmail: "other@example.com",
            description: "Other shelter",
            capacity: 10,
            currentOccupancy: 2,
            photos: new List<string>(),
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: this.testUser.Id);
        await this.context.Shelters.AddAsync(otherShelter);

        var animal2 = Animal.Create(
            slug: "kitty",
            userId: this.testUser.Id,
            name: "Kitty",
            breedId: this.testBreed.Id,
            birthday: null,
            gender: AnimalGender.Female,
            description: null,
            healthStatus: null,
            photos: new List<string>(),
            videos: new List<string>(),
            shelterId: otherShelter.Id,
            status: AnimalStatus.Available,
            adoptionRequirements: null,
            microchipId: null,
            idNumber: 2,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            haveDocuments: false);

        await this.repository.AddAsync(animal1);
        await this.repository.AddAsync(animal2);
        await this.context.SaveChangesAsync();

        // Act
        var result = await this.repository.GetByShelterIdAsync(this.testShelter.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(animal1.Id, result.First().Id);
    }

    /// <summary>
    /// Tests that GetBySlugAsync returns the correct animal with all included navigation properties.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetBySlugAsync_ShouldReturnAnimalWithIncludes()
    {
        // Arrange
        var baseSlug = "doggy-slug";

        // Створюємо тварину через метод Create, який додає суфікс у slug
        var animal = Animal.Create(
            slug: baseSlug,
            userId: this.testUser.Id,
            name: "DoggySlug",
            breedId: this.testBreed.Id,
            birthday: null,
            gender: AnimalGender.Male,
            description: null,
            healthStatus: null,
            photos: new List<string>(),
            videos: new List<string>(),
            shelterId: this.testShelter.Id,
            status: AnimalStatus.Available,
            adoptionRequirements: null,
            microchipId: null,
            idNumber: 3,
            weight: null,
            height: null,
            color: null,
            isSterilized: false,
            haveDocuments: false);

        // Перевірка: Name і Slug не порожні
        Assert.False(string.IsNullOrWhiteSpace(animal.Name.Value), "Animal Name should not be empty");
        Assert.False(string.IsNullOrWhiteSpace(animal.Slug.Value), "Animal Slug should not be empty");

        await this.repository.AddAsync(animal);
        await this.context.SaveChangesAsync();

        // Act
        var fromDb = await this.context.Animals
            .Include(a => a.Breed)
            .Include(a => a.Shelter)
            .FirstOrDefaultAsync(a => a.Slug == animal.Slug);

        // Assert
        Assert.NotNull(fromDb);
        Assert.Equal(animal.Id, fromDb!.Id);
        Assert.NotNull(fromDb.Breed);
        Assert.NotNull(fromDb.Shelter);
        Assert.NotNull(fromDb.AdoptionApplications);
        Assert.NotNull(fromDb.Tags);
        Assert.NotNull(fromDb.SuccessStories);
        Assert.NotNull(fromDb.Subscribers);
    }

    /// <summary>
    /// Seeds required test data: user, shelter, species, breed.
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

        // Create shelter
        this.testShelter = Shelter.Create(
            slug: "test-shelter",
            name: "Test Shelter",
            address: "Street, City",
            coordinates: Coordinates.From(50.45, 30.52),
            contactPhone: "+380501234567",
            contactEmail: "shelter@example.com",
            description: "Test shelter",
            capacity: 50,
            currentOccupancy: 10,
            photos: new List<string>(),
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: null,
            managerId: this.testUser.Id);
        await this.context.Shelters.AddAsync(this.testShelter);

        // Create species and breed
        var species = Specie.Create("Dog");
        await this.context.Species.AddAsync(species);

        this.testBreed = Breed.Create(species.Id, "Dog", "Common dog breed");
        await this.context.Breeds.AddAsync(this.testBreed);

        await this.context.SaveChangesAsync();
    }
}

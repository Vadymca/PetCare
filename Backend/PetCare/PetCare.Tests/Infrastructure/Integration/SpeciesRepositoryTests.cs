namespace PetCare.Tests.Infrastructure.Integration;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;
using PetCare.Domain.Abstractions.Events;
using PetCare.Domain.Aggregates;
using PetCare.Infrastructure.Persistence;
using PetCare.Infrastructure.Persistence.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

/// <summary>
/// Integration tests for <see cref="SpeciesRepository"/>.
/// </summary>
public sealed class SpeciesRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer postgres;
    private AppDbContext context = null!;
    private SpeciesRepository repository = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpeciesRepositoryTests"/> class.
    /// Configures the PostgreSQL test container.
    /// </summary>
    public SpeciesRepositoryTests()
    {
        this.postgres = new PostgreSqlBuilder()
            .WithImage("postgis/postgis:16-3.4")
            .WithDatabase("petcare_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    /// <summary>
    /// Sets up the database and repository before each test.
    /// Starts the PostgreSQL container and creates the schema.
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
            })
            .Options;

        this.context = new AppDbContext(options, dispatcherMock.Object);
        await this.context.Database.EnsureCreatedAsync();

        this.repository = new SpeciesRepository(this.context);
    }

    /// <summary>
    /// Cleans up the database context and stops the PostgreSQL container after tests.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DisposeAsync()
    {
        await this.context.DisposeAsync();
        await this.postgres.StopAsync();
    }

    /// <summary>
    /// Tests that <see cref="SpeciesRepository.GetByNameAsync"/> correctly returns a specie when it exists.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetByNameAsync_ShouldReturnSpecie()
    {
        // Arrange
        var specieName = "Dog";
        var specie = Specie.Create(specieName);
        await this.context.Species.AddAsync(specie);
        await this.context.SaveChangesAsync();

        // Act
        var fromDb = await this.repository.GetByNameAsync(specieName);

        // Assert
        Assert.NotNull(fromDb);
        Assert.Equal(specie.Id, fromDb!.Id);
        Assert.Equal(specieName, fromDb.Name.Value);
    }

    /// <summary>
    /// Tests that <see cref="SpeciesRepository.GetByNameAsync"/> returns null when the specie does not exist.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenSpecieDoesNotExist()
    {
        // Act
        var fromDb = await this.repository.GetByNameAsync("NonExistingSpecie");

        // Assert
        Assert.Null(fromDb);
    }

    /// <summary>
    /// Tests that <see cref="SpeciesRepository.GetAllAsync"/> returns all persisted species.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllSpecies()
    {
        // Arrange
        var speciesList = new List<Specie>
        {
            Specie.Create("Cat"),
            Specie.Create("Dog"),
            Specie.Create("Bird"),
        };

        await this.context.Species.AddRangeAsync(speciesList);
        await this.context.SaveChangesAsync();

        // Act
        var allSpecies = await this.repository.GetAllAsync();

        // Assert
        Assert.Equal(3, allSpecies.Count);
        Assert.Contains(allSpecies, s => s.Name.Value == "Cat");
        Assert.Contains(allSpecies, s => s.Name.Value == "Dog");
        Assert.Contains(allSpecies, s => s.Name.Value == "Bird");
    }
}

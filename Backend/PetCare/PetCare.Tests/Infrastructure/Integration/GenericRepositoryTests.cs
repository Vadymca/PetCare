namespace PetCare.Tests.Infrastructure.Integration;

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using PetCare.Domain.Abstractions.Events;
using PetCare.Domain.Aggregates;
using PetCare.Infrastructure.Persistence;
using Testcontainers.PostgreSql;
using Xunit;

/// <summary>
/// Integration tests for <see cref="GenericRepository{T}"/>.
/// </summary>
public sealed class GenericRepositoryTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer postgres;
    private AppDbContext context = null!;
    private GenericRepository<Specie> repository = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepositoryTests"/> class.
    /// </summary>
    public GenericRepositoryTests()
    {
        this.postgres = new PostgreSqlBuilder()
            .WithImage("postgis/postgis:16-3.4")
            .WithDatabase("petcare_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    /// <summary>
    /// Sets up the database and repository before each test run.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task InitializeAsync()
    {
        await this.postgres.StartAsync();
        var dispatcherMock = new Mock<IDomainEventDispatcher>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(this.postgres.GetConnectionString(), x => x.UseNetTopologySuite())
            .Options;

        this.context = new AppDbContext(options, dispatcherMock.Object);
        await this.context.Database.EnsureCreatedAsync();

        this.repository = new GenericRepository<Specie>(this.context);
    }

    /// <summary>
    /// Disposes the DbContext and stops the Postgres container after tests.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DisposeAsync()
    {
        await this.context.DisposeAsync();
        await this.postgres.StopAsync();
    }

    /// <summary>
    /// Tests that an entity can be added and persisted.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task AddAsync_ShouldPersistEntity()
    {
        var species = Specie.Create("Dog");
        var result = await this.repository.AddAsync(species);

        var fromDb = await this.repository.GetByIdAsync(species.Id);

        Assert.NotNull(result);
        Assert.Equal("Dog", fromDb?.Name.Value);
    }

    /// <summary>
    /// Tests that an existing entity can be updated.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        var species = Specie.Create("Dog");
        await this.repository.AddAsync(species);

        species.Rename("UpdatedDog");
        var result = await this.repository.UpdateAsync(species);

        var fromDb = await this.repository.GetByIdAsync(species.Id);
        Assert.Equal("UpdatedDog", fromDb?.Name.Value);
    }

    /// <summary>
    /// Tests that updating a non-existent entity throws an exception.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenEntityDoesNotExist()
    {
        var species = Specie.Create("Cat");
        await Assert.ThrowsAsync<InvalidOperationException>(() => this.repository.UpdateAsync(species));
    }

    /// <summary>
    /// Tests that an entity can be deleted.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        var species = Specie.Create("Dog");
        await this.repository.AddAsync(species);

        await this.repository.DeleteAsync(species);

        var fromDb = await this.repository.GetByIdAsync(species.Id);
        Assert.Null(fromDb);
    }

    /// <summary>
    /// Tests that GetAllAsync returns all entities in the repository.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        await this.repository.AddAsync(Specie.Create("Dog"));
        await this.repository.AddAsync(Specie.Create("Cat"));

        var result = await this.repository.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    /// <summary>
    /// Tests that CountAsync returns the correct number of entities.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task CountAsync_ShouldReturnCorrectNumber()
    {
        await this.repository.AddAsync(Specie.Create("Dog"));
        await this.repository.AddAsync(Specie.Create("Cat"));

        var count = await this.repository.CountAsync();

        Assert.Equal(2, count);
    }
}

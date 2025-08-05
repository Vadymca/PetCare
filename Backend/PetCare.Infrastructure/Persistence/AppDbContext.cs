// <copyright file="AppDbContext.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Infrastructure.Persistence.Configurations;

/// <summary>
/// Represents the application's database context.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the species entities.
    /// </summary>
    public DbSet<Specie> Species => this.Set<Specie>();

    /// <summary>
    /// Gets the breed entities.
    /// </summary>
    public DbSet<Breed> Breeds => this.Set<Breed>();

    /// <summary>
    /// Gets the shelter entities.
    /// </summary>
    public DbSet<Shelter> Shelters => this.Set<Shelter>();

    /// <summary>
    /// Gets the animal entities.
    /// </summary>
    public DbSet<Animal> Animals => this.Set<Animal>();

    /// <summary>
    /// Gets the user entities.
    /// </summary>
    public DbSet<User> Users => this.Set<User>();

    /// <summary>
    /// Configures the model by applying entity configurations.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new SpeciesConfiguration());
        modelBuilder.ApplyConfiguration(new BreedConfiguration());
        modelBuilder.ApplyConfiguration(new ShelterConfiguration());
        modelBuilder.ApplyConfiguration(new AnimalConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}

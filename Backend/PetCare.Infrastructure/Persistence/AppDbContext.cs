using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Infrastructure.Persistence.Configurations;

namespace PetCare.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Specie> Species { get; set; } = null!;
        public DbSet<Breed> Breeds { get; set; } = null!;
        public DbSet<Shelter> Shelters { get; set; } = null!;
        public DbSet<Animal> Animals { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Підключення конфігурацій
            modelBuilder.ApplyConfiguration(new SpeciesConfiguration());
            modelBuilder.ApplyConfiguration(new BreedConfiguration());
            modelBuilder.ApplyConfiguration(new ShelterConfiguration());
            modelBuilder.ApplyConfiguration(new AnimalConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

    }
}

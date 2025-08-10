namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Configures the <see cref="Breed"/> entity for Entity Framework Core.
/// </summary>
public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    /// <summary>
    /// Configures the entity type builder for <see cref="Breed"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("Breeds");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(b => b.Name)
            .HasConversion(
            name => name.Value,
            value => Name.Create(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.Description)
            .HasColumnType("text");

        builder.Property(b => b.SpeciesId)
            .HasColumnName("species_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.HasOne(b => b.Specie)
            .WithMany(s => s.Breeds)
            .HasForeignKey(b => b.SpeciesId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => b.SpeciesId);

        builder.HasIndex(b => new { b.Name, b.SpeciesId })
            .IsUnique();
    }
}

namespace PetCare.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;
using PetCare.Domain.Aggregates;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Configures the <see cref="Specie"/> entity for Entity Framework Core.
/// </summary>
public class SpeciesConfiguration : IEntityTypeConfiguration<Specie>
{
    /// <summary>
    /// Configures the entity type builder for <see cref="Specie"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Specie> builder)
    {
        builder.ToTable("Species");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.Name)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(s => s.Name)
            .IsUnique();

        builder.Property<NpgsqlTsVector>("SearchVector")
            .HasColumnType("tsvector")
            .HasComputedColumnSql(
                """
                to_tsvector('simple', coalesce("Name", ''))
                ||
                to_tsvector('english', coalesce("Name", ''))
                """,
                stored: true);

        builder.HasIndex("SearchVector")
            .HasMethod("GIN");
    }
}

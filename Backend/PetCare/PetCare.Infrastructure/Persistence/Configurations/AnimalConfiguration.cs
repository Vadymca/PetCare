namespace PetCare.Infrastructure.Persistence.Configurations;

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Configures the Animal entity mapping and constraints.
/// </summary>
public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.ToTable("Animals", t =>
        {
            t.HasCheckConstraint("CK_Animals_Weight", "\"Weight\" > 0");
            t.HasCheckConstraint("CK_Animals_Height", "\"Height\" > 0");
        });

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(a => a.Slug)
           .HasConversion(
                slug => slug.Value,
                value => Slug.FromExisting(value))
           .HasMaxLength(256)
           .IsRequired();

        builder.HasIndex(a => a.Slug)
            .IsUnique();

        builder.Property(a => a.UserId);

        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(a => a.Name)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.BreedId)
           .IsRequired();

        builder.HasOne(a => a.Breed)
            .WithMany()
            .HasForeignKey(a => a.BreedId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.Birthday)
           .HasConversion(
               birthday => birthday!.Value,
               value => Birthday.Create(value))
           .HasColumnType("timestamp with time zone");

        builder.Property(a => a.Gender)
            .HasColumnType("animal_gender")
            .IsRequired();

        builder.Property(a => a.Description)
            .IsRequired(false);

        builder.Property(a => a.Size)
             .HasColumnType("animal_size")
             .IsRequired();

        builder.Property(a => a.SpecialNeeds)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
            .HasColumnType("jsonb");

        builder.Property(a => a.HealthConditions)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
            .HasColumnType("jsonb");

        builder.Property(a => a.Temperaments)
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() },
                }),
                v => JsonSerializer.Deserialize<List<AnimalTemperament>>(v, new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() },
                }) ?? new List<AnimalTemperament>())
            .HasColumnType("jsonb");

        builder.Property(a => a.Photos)
           .HasColumnType("jsonb")
           .IsRequired(false);

        builder.Property(a => a.Videos)
            .HasColumnType("jsonb")
            .IsRequired(false);

        builder.Property(a => a.ShelterId)
           .IsRequired();

        builder.HasOne(a => a.Shelter)
            .WithMany()
            .HasForeignKey(a => a.ShelterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(a => a.Status)
           .HasColumnType("animal_status")
           .IsRequired();

        builder.Property(a => a.CareCost)
           .HasColumnType("int")
           .IsRequired()
           .HasDefaultValue(AnimalCareCost.SixHundred);

        builder.Property(a => a.AdoptionRequirements)
           .IsRequired(false);

        builder.Property(a => a.MicrochipId)
           .HasConversion(
           microchipId => microchipId!.Value,
           value => MicrochipId.Create(value))
           .HasMaxLength(50);

        builder.HasIndex(a => a.MicrochipId)
            .IsUnique();

        builder.Property(a => a.Weight)
            .IsRequired(false);

        builder.Property(a => a.Height)
            .IsRequired(false);

        builder.Property(a => a.Color)
            .HasMaxLength(50);

        builder.Property(a => a.IsSterilized)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(a => a.IsUnderCare)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(a => a.HaveDocuments)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(a => a.CreatedAt)
           .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(a => a.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(a => a.BreedId);
        builder.HasIndex(a => a.ShelterId);

        builder.HasMany(a => a.Tags)
           .WithMany()
           .UsingEntity<Dictionary<string, object>>(
               "AnimalTags",
               j => j
                   .HasOne<Tag>()
                   .WithMany()
                   .HasForeignKey("TagId")
                   .OnDelete(DeleteBehavior.Cascade),
               j => j
                   .HasOne<Animal>()
                   .WithMany()
                   .HasForeignKey("AnimalId")
                   .OnDelete(DeleteBehavior.Cascade),
               j =>
               {
                   j.HasKey("AnimalId", "TagId");
               });

        // --- Full-text search vector ---
        builder.Property<NpgsqlTsVector>("SearchVector")
            .HasColumnType("tsvector")
            .HasComputedColumnSql(
            @"
            to_tsvector('simple', coalesce(""Name"",'') || ' ' || coalesce(""Description"",''))
            || to_tsvector('english', coalesce(""Name"",'') || ' ' || coalesce(""Description"",''))
        ",
            stored: true);

        builder.HasIndex("SearchVector")
            .HasMethod("GIN");
    }
}

namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
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
                value => Slug.Create(value))
           .HasMaxLength(64)
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
                birthday => birthday.Value,
                value => Birthday.Create(value));

        builder.Property(a => a.Gender)
            .HasColumnType("animal_gender")
            .IsRequired();

        builder.Property(a => a.Description)
            .IsRequired(false);

        builder.Property(a => a.HealthStatus)
            .IsRequired(false);

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

        builder.Property(a => a.AdoptionRequirements)
           .IsRequired(false);

        builder.Property(a => a.MicrochipId)
           .HasConversion(
           microchipId => microchipId.Value,
           value => MicrochipId.Create(value))
           .HasMaxLength(50);

        builder.HasIndex(a => a.MicrochipId)
            .IsUnique();

        builder.Property(a => a.IdNumber)
            .IsRequired();

        builder.HasIndex(a => new { a.ShelterId, a.IdNumber })
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
    }
}

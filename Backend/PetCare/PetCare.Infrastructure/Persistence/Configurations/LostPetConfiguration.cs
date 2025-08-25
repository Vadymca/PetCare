namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;
using PetCare.Domain.ValueObjects;

/// <summary>LostPets config.</summary>
public sealed class LostPetConfiguration : IEntityTypeConfiguration<LostPet>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<LostPet> builder)
    {
        builder.ToTable("LostPets", t =>
        {
            t.HasCheckConstraint("CK_LostPets_Reward", "\"Reward\" >= 0");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Slug)
            .HasConversion(
                slug => slug.Value,
                value => Slug.Create(value))
            .HasMaxLength(64)
            .IsRequired();
        builder.HasIndex(x => x.Slug).IsUnique();

        builder.Property(x => x.Name)
             .HasConversion(
                name => name.Value,
                value => Name.Create(value))
            .HasMaxLength(50);
        builder.Property(x => x.Description);

        builder.Property(x => x.LastSeenLocation)
            .HasConversion(
                 c => c.Point,
                 p => Coordinates.From(p.Y, p.X))
            .HasColumnType("geometry (Point, 4326)");
        builder.Property(x => x.LastSeenDate).HasColumnType("timestamptz");

        builder.Property(x => x.Photos).HasColumnType("jsonb");
        builder.Property(x => x.Status).HasColumnType("lost_pet_status").IsRequired();

        builder.Property(x => x.AdminNotes);
        builder.Property(x => x.Reward);
        builder.Property(x => x.ContactAlternative).HasMaxLength(255);
        builder.Property(x => x.MicrochipId)
             .HasConversion(
                microchipId => microchipId.Value,
                value => MicrochipId.Create(value))
            .HasMaxLength(50);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.User)
            .WithMany(u => u.LostPets)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Breed)
            .WithMany()
            .HasForeignKey(x => x.BreedId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.LastSeenDate);
    }
}

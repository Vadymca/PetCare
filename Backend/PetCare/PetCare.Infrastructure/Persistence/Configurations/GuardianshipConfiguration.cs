namespace PetCare.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Aggregates;

/// <summary>
/// Configures the Guardianship aggregate mapping and constraints.
/// </summary>
public sealed class GuardianshipConfiguration : IEntityTypeConfiguration<Guardianship>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Guardianship> builder)
    {
        builder.ToTable("Guardianships", t =>
        {
            t.HasCheckConstraint("CK_Guardianships_StartDate", "\"StartDate\" <= NOW()");
        });

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(g => g.UserId)
            .IsRequired();

        builder.HasOne(g => g.User)
            .WithMany()
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(g => g.AnimalId)
            .IsRequired();

        builder.HasOne(g => g.Animal)
            .WithMany()
            .HasForeignKey(g => g.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(g => g.Status)
            .HasColumnType("guardianship_status")
            .IsRequired();

        builder.Property(g => g.StartDate)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(g => g.GraceUntil)
            .HasColumnType("timestamp with time zone");

        builder.Property(g => g.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(g => g.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(g => g.UserId);
        builder.HasIndex(g => g.AnimalId);
        builder.HasIndex(g => g.Status);
    }
}

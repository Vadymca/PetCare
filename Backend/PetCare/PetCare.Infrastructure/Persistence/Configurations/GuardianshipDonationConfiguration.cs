namespace PetCare.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>
/// Configures the GuardianshipDonation entity mapping.
/// </summary>
public sealed class GuardianshipDonationConfiguration : IEntityTypeConfiguration<GuardianshipDonation>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<GuardianshipDonation> builder)
    {
        builder.ToTable("GuardianshipDonations");

        builder.HasKey(gd => gd.Id);

        builder.Property(gd => gd.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(gd => gd.GuardianshipId)
            .IsRequired();

        builder.HasOne(gd => gd.Guardianship)
            .WithMany(g => g.Donations)
            .HasForeignKey(gd => gd.GuardianshipId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(gd => gd.DonationId)
            .IsRequired();

        builder.HasOne(gd => gd.Donation)
            .WithMany()
            .HasForeignKey(gd => gd.DonationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(gd => gd.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(gd => gd.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(gd => new { gd.GuardianshipId, gd.DonationId })
            .IsUnique();
    }
}

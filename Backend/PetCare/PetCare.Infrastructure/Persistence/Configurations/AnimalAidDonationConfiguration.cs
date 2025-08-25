namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>AnimalAidDonations (link) config.</summary>
public sealed class AnimalAidDonationConfiguration : IEntityTypeConfiguration<AnimalAidDonation>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AnimalAidDonation> builder)
    {
        builder.ToTable("AnimalAidDonations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Donation)
            .WithMany(d => d.AnimalAidLinks)
            .HasForeignKey(x => x.DonationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AnimalAidRequest)
            .WithMany(r => r.Donations)
            .HasForeignKey(x => x.AnimalAidRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

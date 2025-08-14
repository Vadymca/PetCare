namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>Donations config.</summary>
public sealed class DonationConfiguration : IEntityTypeConfiguration<Donation>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Donation> builder)
    {
        builder.ToTable("Donations", t =>
        {
            t.HasCheckConstraint("CK_Donations_Amount", "\"Amount\" > 0");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Amount).IsRequired();
        builder.Property(x => x.Status).HasColumnType("donation_status").IsRequired();

        builder.Property(x => x.TransactionId).HasMaxLength(255);
        builder.Property(x => x.Purpose).HasMaxLength(255);

        builder.Property(x => x.Recurring).HasDefaultValue(false).IsRequired();
        builder.Property(x => x.Anonymous).HasDefaultValue(false).IsRequired();

        builder.Property(x => x.DonationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.Report);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.User)
            .WithMany(u => u.Donations)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Shelter)
            .WithMany(s => s.Donations)
            .HasForeignKey(x => x.ShelterId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.PaymentMethod)
            .WithMany()
            .HasForeignKey(x => x.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.DonationDate);
        builder.HasIndex(x => x.Status);
    }
}

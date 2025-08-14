namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Aggregates;

/// <summary>AdoptionApplications config.</summary>
public sealed class AdoptionApplicationConfiguration : IEntityTypeConfiguration<AdoptionApplication>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AdoptionApplication> builder)
    {
        builder.ToTable("AdoptionApplications");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Status).HasColumnType("adoption_status").IsRequired();

        builder.Property(x => x.ApplicationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.Comment);
        builder.Property(x => x.AdminNotes);
        builder.Property(x => x.RejectionReason);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.User)
            .WithMany(u => u.AdoptionApplications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Animal)
            .WithMany(a => a.AdoptionApplications)
            .HasForeignKey(x => x.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ApprovedByUser)
            .WithMany()
            .HasForeignKey(x => x.ApprovedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.ApplicationDate);
    }
}

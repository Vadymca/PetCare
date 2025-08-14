namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;
using PetCare.Domain.ValueObjects;

/// <summary>AnimalAidRequests config.</summary>
public sealed class AnimalAidRequestConfiguration : IEntityTypeConfiguration<AnimalAidRequest>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AnimalAidRequest> builder)
    {
        builder.ToTable("AnimalAidRequests", t =>
        {
            t.HasCheckConstraint("CK_Aid_EstimatedCost", "\"EstimatedCost\" >= 0");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Title)
            .HasConversion(
                title => title.Value,
                value => Title.Create(value))
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description);

        builder.Property(x => x.Category).HasColumnType("aid_category").IsRequired();
        builder.Property(x => x.Status).HasColumnType("aid_status").IsRequired();

        builder.Property(x => x.EstimatedCost);
        builder.Property(x => x.Photos).HasColumnType("jsonb");

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.User)
            .WithMany(u => u.AnimalAidRequests)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Shelter)
            .WithMany(s => s.AnimalAidRequests)
            .HasForeignKey(x => x.ShelterId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Category);
        builder.HasIndex(x => x.CreatedAt);
    }
}

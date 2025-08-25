namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;
using PetCare.Domain.ValueObjects;

/// <summary>SuccessStories config.</summary>
public sealed class SuccessStoryConfiguration : IEntityTypeConfiguration<SuccessStory>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<SuccessStory> builder)
    {
        builder.ToTable("SuccessStories", t =>
        {
            t.HasCheckConstraint("CK_SuccessStories_Views", "\"Views\" >= 0");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Title)
            .HasConversion(
                title => title.Value,
                value => Title.Create(value))
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.Content).IsRequired();

        builder.Property(x => x.Photos).HasColumnType("jsonb");
        builder.Property(x => x.Videos).HasColumnType("jsonb");

        builder.Property(x => x.PublishedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.Views).HasDefaultValue(0).IsRequired();

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Animal)
            .WithMany(a => a.SuccessStories)
            .HasForeignKey(x => x.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(u => u.SuccessStories)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.PublishedAt);
    }
}

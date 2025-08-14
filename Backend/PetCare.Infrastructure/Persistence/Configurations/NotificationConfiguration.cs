namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;
using PetCare.Domain.ValueObjects;

/// <summary>Notifications config.</summary>
public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Title)
            .HasConversion(
                title => title.Value,
                value => Title.Create(value))
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.Message).IsRequired();

        builder.Property(x => x.IsRead).HasDefaultValue(false).IsRequired();

        builder.Property(x => x.NotifiableEntity).HasMaxLength(50);
        builder.Property(x => x.NotifiableEntityId);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.NotificationType)
            .WithMany()
            .HasForeignKey(x => x.NotificationTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.UserId, x.IsRead });
        builder.HasIndex(x => x.CreatedAt);
    }
}

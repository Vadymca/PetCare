namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>AnimalSubscriptions (link) config.</summary>
public sealed class AnimalSubscriptionConfiguration : IEntityTypeConfiguration<AnimalSubscription>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AnimalSubscription> builder)
    {
        builder.ToTable("AnimalSubscriptions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SubscribedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.User)
            .WithMany(u => u.AnimalSubscriptions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Animal)
            .WithMany(a => a.Subscribers)
            .HasForeignKey(x => x.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

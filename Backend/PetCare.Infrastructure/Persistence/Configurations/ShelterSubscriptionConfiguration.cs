namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>ShelterSubscriptions (link) config.</summary>
public sealed class ShelterSubscriptionConfiguration : IEntityTypeConfiguration<ShelterSubscription>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ShelterSubscription> builder)
    {
        builder.ToTable("ShelterSubscriptions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SubscribedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.User)
            .WithMany(u => u.ShelterSubscriptions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Shelter)
            .WithMany(s => s.Subscribers)
            .HasForeignKey(x => x.ShelterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

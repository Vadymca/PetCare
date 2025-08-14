namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>GamificationRewards config.</summary>
public sealed class GamificationRewardConfiguration : IEntityTypeConfiguration<GamificationReward>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<GamificationReward> builder)
    {
        builder.ToTable("GamificationRewards", t =>
        {
            t.HasCheckConstraint("CK_GamificationRewards_Points", "\"Points\" >= 0");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Points).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(255);
        builder.Property(x => x.Used).HasDefaultValue(false).IsRequired();

        builder.Property(x => x.AwardedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.User)
            .WithMany(u => u.GamificationRewards)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Task)
            .WithMany(t => t.Rewards)
            .HasForeignKey(x => x.TaskId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

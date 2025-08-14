namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>
/// Configures the <see cref="Like"/> entity for Entity Framework Core.
/// </summary>
public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    /// <summary>
    /// Configures the entity type builder for <see cref="Like"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.ToTable("Likes", t =>
        {
            t.HasCheckConstraint("CK_Likes_UserId", "\"UserId\" IS NOT NULL");
            t.HasCheckConstraint("CK_Likes_LikedEntity", "length(\"LikedEntity\") > 0");
            t.HasCheckConstraint("CK_Likes_LikedEntityId", "\"LikedEntityId\" IS NOT NULL");
        });

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(l => l.UserId).IsRequired();

        builder.HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(l => l.LikedEntity).HasMaxLength(100).IsRequired();

        builder.Property(l => l.LikedEntityId).IsRequired();

        builder.Property(l => l.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();

        builder.HasIndex(l => new { l.UserId, l.LikedEntity, l.LikedEntityId }).IsUnique();
    }
}

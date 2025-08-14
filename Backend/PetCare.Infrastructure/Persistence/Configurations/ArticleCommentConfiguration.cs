namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>ArticleComments config.</summary>
public sealed class ArticleCommentConfiguration : IEntityTypeConfiguration<ArticleComment>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<ArticleComment> builder)
    {
        builder.ToTable("ArticleComments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.Status).HasColumnType("comment_status").IsRequired();

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Article)
            .WithMany(a => a.Comments)
            .HasForeignKey(x => x.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(u => u.ArticleComments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ParentComment)
            .WithMany(p => p.Replies)
            .HasForeignKey(x => x.ParentCommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ModeratedBy)
            .WithMany()
            .HasForeignKey(x => x.ModeratedById)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(c => c.Likes)
            .WithOne(l => l.ArticleComment)
            .HasForeignKey(l => l.ArticleCommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.CreatedAt);

    }
}

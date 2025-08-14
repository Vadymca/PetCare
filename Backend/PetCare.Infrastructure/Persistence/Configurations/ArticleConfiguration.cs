namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;
using PetCare.Domain.ValueObjects;

/// <summary>Articles config.</summary>
public sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Articles");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Title)
             .HasConversion(
                title => title.Value,
                value => Title.Create(value))
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(x => x.Content).IsRequired();

        builder.Property(x => x.Status).HasColumnType("article_status").IsRequired();
        builder.Property(x => x.Thumbnail).HasMaxLength(255);

        builder.Property(x => x.PublishedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Author)
            .WithMany(u => u.Articles)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.PublishedAt);
    }
}

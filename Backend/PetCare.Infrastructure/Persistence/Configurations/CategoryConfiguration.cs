namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;
using PetCare.Domain.ValueObjects;

/// <summary>Categories config.</summary>
public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Name)
             .HasConversion(
                name => name.Value,
                value => Name.Create(value))
            .HasMaxLength(50)
            .IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Description);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}

namespace PetCare.Infrastructure.Persistence.Configurations;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Aggregates;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Configures the <see cref="User"/> entity for Entity Framework Core.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the entity type builder for <see cref="User"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    [Obsolete]
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasCheckConstraint("CK_Users_Points", "\"Points\" >= 0");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(u => u.FirstName)
           .HasMaxLength(50)
           .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Phone)
            .HasMaxLength(30)
            .IsRequired(false);

        builder.HasIndex(u => u.Phone)
            .IsUnique()
            .HasFilter("\"Phone\" IS NOT NULL");

        builder.Property(u => u.Role)
            .HasColumnType("user_role")
            .IsRequired();

        builder.Property(u => u.Preferences)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null) ?? new Dictionary<string, string>());

        builder.Property(u => u.Points)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(u => u.LastLogin)
            .HasColumnType("timestamptz");

        builder.Property(u => u.ProfilePhoto)
            .HasMaxLength(255);

        builder.Property(u => u.Language)
            .HasDefaultValue("uk")
            .IsRequired();

        builder.Property(u => u.PostalCode)
          .HasMaxLength(20);

        builder.Property(u => u.Address)
           .HasConversion(
               addr => addr!.ToString(),
               str => Address.Create(str))
           .IsRequired(false);

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(u => u.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken(false);
    }
}

namespace PetCare.Infrastructure.Persistence.Configurations;
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
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", t =>
        {
            t.HasCheckConstraint("CK_Users_Role", "\"Role\" IN ('User', 'Admin', 'Moderator')");
            t.HasCheckConstraint("CK_Users_Points", "\"Points\" >= 0");
        });

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(u => u.Email)
            .HasConversion(
            email => email.Value,
            value => Email.Create(value))
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.FirstName)
           .HasMaxLength(50)
           .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Phone)
            .HasConversion(
            phone => phone.Value,
            value => PhoneNumber.Create(value))
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(u => u.Phone)
            .IsUnique();

        builder.Property(u => u.Role)
            .HasColumnType("varchar(20)")
            .IsRequired();

        builder.Property(u => u.Preferences)
            .HasColumnType("jsonb");

        builder.Property(u => u.Points)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(u => u.LastLogin)
            .HasColumnType("timestamptz");

        builder.Property(u => u.ProfilePhoto)
            .HasMaxLength(255);

        builder.Property(u => u.Language)
            .HasColumnType("varchar(10)")
            .HasDefaultValue("uk")
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(u => u.UpdatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}

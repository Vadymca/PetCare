namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Aggregates;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Configures the <see cref="Shelter"/> entity for Entity Framework Core.
/// </summary>
public class ShelterConfiguration : IEntityTypeConfiguration<Shelter>
{
    /// <summary>
    /// Configures the entity type builder for <see cref="Shelter"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Shelter> builder)
    {
        builder.ToTable("Shelters", t =>
        {
            t.HasCheckConstraint("CK_Shelters_Capacity", "\"Capacity\" >= 0");
        });

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.Slug)
            .HasConversion(
                slug => slug.Value,
                value => Slug.Create(value))
            .HasMaxLength(64)
            .IsRequired();

        builder.HasIndex(s => s.Slug)
            .IsUnique();

        builder.Property(s => s.Name)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Address)
            .HasConversion(
                addr => addr.ToString(),
                str => Address.Create(str))
            .IsRequired();

        builder.OwnsOne(s => s.Coordinates, cb =>
        {
            cb.Property(c => c.Point)
              .HasColumnType("geometry (point,4326)")
              .IsRequired();
        });

        builder.HasIndex(s => s.Coordinates)
            .HasMethod("GIST");

        builder.Property(s => s.ContactPhone)
            .HasConversion(
                phone => phone.Value,
                value => PhoneNumber.Create(value))
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.ContactEmail)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value))
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(s => s.Description)
             .IsRequired(false);

        builder.Property(s => s.Capacity)
            .IsRequired();

        builder.Property(s => s.CurrentOccupancy)
            .IsRequired();

        builder.Property(s => s.Photos)
            .HasColumnType("jsonb");

        builder.Property(s => s.VirtualTourUrl)
            .HasMaxLength(255);

        builder.Property(s => s.WorkingHours)
            .HasMaxLength(100);

        builder.Property(s => s.SocialMedia)
            .HasColumnType("jsonb");

        builder.Property(s => s.ManagerId)
            .IsRequired(false);

        builder.HasOne(s => s.Manager)
            .WithMany()
            .HasForeignKey(s => s.ManagerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(s => s.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(s => s.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}

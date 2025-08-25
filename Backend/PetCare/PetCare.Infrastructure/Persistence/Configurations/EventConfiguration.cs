namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;
using PetCare.Domain.ValueObjects;

/// <summary>Events config.</summary>
public sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Title)
             .HasConversion(
                title => title.Value,
                value => Title.Create(value))
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.Description);

        builder.Property(x => x.EventDate).HasColumnType("timestamptz");
        builder.Property(x => x.Location)
            .HasConversion(
                c => c.Point,
                p => Coordinates.From(p.Y, p.X))
            .HasColumnType("geometry(Point, 4326)");
        builder.Property(x => x.Address)
            .HasConversion(
                adr => adr.ToString(),
                str => Address.Create(str));

        builder.Property(x => x.Type).HasColumnType("event_type").IsRequired();
        builder.Property(x => x.Status).HasColumnType("event_status").IsRequired();

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Shelter)
            .WithMany(s => s.Events)
            .HasForeignKey(x => x.ShelterId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.EventDate);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Type);
    }
}

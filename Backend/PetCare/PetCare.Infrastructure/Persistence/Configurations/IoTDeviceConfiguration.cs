namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>IoTDevices config.</summary>
public sealed class IoTDeviceConfiguration : IEntityTypeConfiguration<IoTDevice>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<IoTDevice> builder)
    {
        builder.ToTable("IoTDevices");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Type).HasColumnType("io_t_device_type").IsRequired();
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Status).HasColumnType("io_t_device_status").IsRequired();

        builder.Property(x => x.Data).HasColumnType("jsonb");
        builder.Property(x => x.SerialNumber).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.SerialNumber).IsUnique();

        builder.Property(x => x.AlertThresholds).HasColumnType("jsonb");

        builder.Property(x => x.LastUpdated).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Shelter)
            .WithMany(s => s.IoTDevices)
            .HasForeignKey(x => x.ShelterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Type);
    }
}

namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>AuditLogs config.</summary>
public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.TableName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.RecordId).IsRequired();
        builder.Property(x => x.Operation).HasColumnType("audit_operation").IsRequired();

        builder.Property(x => x.Changes).HasColumnType("jsonb");
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.TableName);
        builder.HasIndex(x => x.RecordId);
        builder.HasIndex(x => new { x.TableName, x.RecordId });
    }
}

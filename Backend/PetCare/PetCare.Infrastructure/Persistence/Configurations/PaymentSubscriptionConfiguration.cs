namespace PetCare.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>
/// Configures the PaymentSubscription entity mapping.
/// </summary>
public sealed class PaymentSubscriptionConfiguration : IEntityTypeConfiguration<PaymentSubscription>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PaymentSubscription> builder)
    {
        builder.ToTable("PaymentSubscriptions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.PaymentMethodId)
            .IsRequired();

        builder.HasOne(p => p.PaymentMethod)
            .WithMany()
            .HasForeignKey(p => p.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.ScopeType)
            .HasColumnType("subscription_scope")
            .IsRequired();

        builder.Property(p => p.Status)
            .HasColumnType("subscription_status")
            .IsRequired();

        builder.Property(p => p.ScopeId)
            .IsRequired(false);

        builder.Property(p => p.Amount)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(p => p.Currency)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(p => p.Provider)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.ProviderSubscriptionId)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.NextChargeAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);

        builder.Property(p => p.LastChargeAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);

        builder.Property(p => p.CanceledAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(p => p.ProviderSubscriptionId)
            .IsUnique();

        builder.HasIndex(p => new { p.UserId, p.ScopeType, p.ScopeId })
            .HasDatabaseName("IX_PaymentSubscriptions_Scope");
    }
}

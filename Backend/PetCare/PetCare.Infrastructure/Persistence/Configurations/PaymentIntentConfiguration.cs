namespace PetCare.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Aggregates;

/// <summary>
/// Configures the <see cref="PaymentIntent"/> aggregate mapping and database constraints.
/// </summary>
public sealed class PaymentIntentConfiguration : IEntityTypeConfiguration<PaymentIntent>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PaymentIntent> builder)
    {
        builder.ToTable("PaymentIntents", t =>
        {
            t.HasCheckConstraint("CK_PaymentIntents_Amount", "\"Amount\" > 0");
        });

        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        // --- ExternalOrderId ---
        builder.Property(pi => pi.ExternalOrderId)
            .IsRequired()
            .HasMaxLength(64);

        builder.HasIndex(pi => pi.ExternalOrderId)
            .IsUnique();

        // --- Provider ---
        builder.Property(pi => pi.PaymentProvider)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(pi => pi.ProviderPaymentId)
            .HasMaxLength(128);

        // --- Scope ---
        builder.Property(pi => pi.ScopeType)
            .HasColumnType("subscription_scope");

        builder.Property(pi => pi.ScopeId);

        // --- User ---
        builder.Property(pi => pi.UserId);

        builder.HasOne(pi => pi.User)
            .WithMany()
            .HasForeignKey(pi => pi.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Amount & Currency ---
        builder.Property(pi => pi.Amount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(pi => pi.Currency)
            .IsRequired()
            .HasMaxLength(8);

        // --- Recurring / Anonymous flags ---
        builder.Property(pi => pi.IsRecurring)
            .IsRequired();

        builder.Property(pi => pi.Anonymous)
            .IsRequired();

        builder.Property(pi => pi.PayerName)
            .HasMaxLength(100)
            .IsRequired(false);

        // --- Status ---
        builder.Property(pi => pi.Status)
            .IsRequired()
            .HasColumnType("payment_intent_status");

        // --- Timestamps ---
        builder.Property(pi => pi.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(pi => pi.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        // --- Donation (optional 1:1) ---
        builder.HasOne(pi => pi.Donation)
            .WithOne()
            .HasForeignKey<PaymentIntent>(pi => pi.DonationId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Subscription (optional 1:1) ---
        builder.HasOne(pi => pi.Subscription)
            .WithOne()
            .HasForeignKey<PaymentIntent>(pi => pi.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Guardianship (optional 1:1) ---
        builder.HasOne(pi => pi.Guardianship)
            .WithOne()
            .HasForeignKey<PaymentIntent>(pi => pi.GuardianshipId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Indexes ---
        builder.HasIndex(pi => pi.UserId);
        builder.HasIndex(pi => pi.ScopeType);
        builder.HasIndex(pi => pi.ScopeId);
        builder.HasIndex(pi => pi.Status);
    }
}

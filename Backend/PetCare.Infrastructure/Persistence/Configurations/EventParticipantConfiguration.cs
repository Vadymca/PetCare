namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>EventParticipants (link) config.</summary>
public sealed class EventParticipantConfiguration : IEntityTypeConfiguration<EventParticipant>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<EventParticipant> builder)
    {
        builder.ToTable("EventParticipants");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RegisteredAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Event)
            .WithMany(e => e.Participants)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(u => u.EventParticipations)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

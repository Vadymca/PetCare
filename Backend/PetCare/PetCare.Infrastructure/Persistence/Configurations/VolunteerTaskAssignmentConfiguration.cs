namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;

/// <summary>VolunteerTaskAssignments config.</summary>
public sealed class VolunteerTaskAssignmentConfiguration : IEntityTypeConfiguration<VolunteerTaskAssignment>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<VolunteerTaskAssignment> builder)
    {
        builder.ToTable("VolunteerTaskAssignments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AssignedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.VolunteerTask)
            .WithMany(t => t.Assignments)
            .HasForeignKey(x => x.VolunteerTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(u => u.VolunteerTaskAssignments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

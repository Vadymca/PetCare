namespace PetCare.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Aggregates;
using PetCare.Domain.ValueObjects;

/// <summary>VolunteerTasks config.</summary>
public sealed class VolunteerTaskConfiguration : IEntityTypeConfiguration<VolunteerTask>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<VolunteerTask> builder)
    {
        builder.ToTable("VolunteerTasks", t =>
        {
            t.HasCheckConstraint("CK_VolunteerTasks_Duration", "\"Duration\" > 0");
            t.HasCheckConstraint("CK_VolunteerTasks_Required", "\"RequiredVolunteers\" > 0");
            t.HasCheckConstraint("CK_VolunteerTasks_Points", "\"PointsReward\" >= 0");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Title)
            .HasConversion(
                title => title.Value,
                value => Title.Create(value))
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description);

        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.Duration);
        builder.Property(x => x.RequiredVolunteers);
        builder.Property(x => x.Status).HasColumnType("volunteer_task_status").IsRequired();

        builder.Property(x => x.PointsReward).IsRequired();

        builder.Property(x => x.Location)
             .HasConversion(
                 c => c.Point,
                 p => Coordinates.From(p.Y, p.X))
            .HasColumnType("geometry(Point, 4326)");
        builder.Property(x => x.SkillsRequired).HasColumnType("jsonb");

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Shelter)
            .WithMany(s => s.VolunteerTasks)
            .HasForeignKey(x => x.ShelterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Date);
        builder.HasIndex(x => x.Status);
    }
}

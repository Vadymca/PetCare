using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Aggregates;
using PetCare.Domain.ValueObjects;

namespace PetCare.Infrastructure.Persistence.Configurations
{
    public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
    {
        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.ToTable("Animals", t =>
            {
                t.HasCheckConstraint("CK_Animals_Weight", "\"Weight\" > 0");
                t.HasCheckConstraint("CK_Animals_Height", "\"Height\" > 0");
                t.HasCheckConstraint("CK_Animals_Gender", "\"Gender\" IN ('Male', 'Female', 'Unknown')");
                t.HasCheckConstraint("CK_Animals_Status", "\"Status\" IN ('Available', 'Adopted', 'Reserved', 'InTreatment')");
            });

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.Slug)
               .HasConversion(
                    slug => slug.Value, 
                    value => Slug.Create(value))
               .HasMaxLength(64)
               .IsRequired();

            builder.HasIndex(a => a.Slug)
                .IsUnique();

            builder.Property(a => a.UserId)
               .HasColumnType("uuid");

            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(a => a.Name)
                .HasConversion(
                    name => name.Value, 
                    value => Name.Create(value))
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(a => a.BreedId)
               .HasColumnType("uuid")
               .IsRequired();

            builder.HasOne(a => a.Breed)
                .WithMany()
                .HasForeignKey(a => a.BreedId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(a => a.Birthday)
                .HasColumnType("date");

            builder.Property(a => a.Gender)
                .HasColumnType("varchar(10)")
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnType("text");

            builder.Property(a => a.HealthStatus)
                .HasColumnType("text");

            builder.Property(a => a.Photos)
               .HasColumnType("jsonb");

            builder.Property(a => a.Videos)
                .HasColumnType("jsonb");

            builder.Property(a => a.ShelterId)
               .HasColumnType("uuid")
               .IsRequired();

            builder.HasOne(a => a.Shelter)
                .WithMany()
                .HasForeignKey(a => a.ShelterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.Status)
               .HasColumnType("varchar(20)")
               .IsRequired();

            builder.Property(a => a.AdoptionRequirements)
                .HasColumnType("text");

            builder.Property(a => a.MicrochipId)
               .HasConversion(
               microchipId => microchipId.Value,
               value => MicrochipId.Create(value))
               .HasMaxLength(50);

            builder.HasIndex(a => a.MicrochipId)
                .IsUnique();

            builder.Property(a => a.IdNumber)
                .IsRequired();

            builder.HasIndex(a => new { a.ShelterId, a.IdNumber })
                .IsUnique();

            builder.Property(a => a.Weight)
                .HasColumnType("float");

            builder.Property(a => a.Height)
                .HasColumnType("float");

            builder.Property(a => a.Color)
                .HasMaxLength(50);

            builder.Property(a => a.IsSterilized)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(a => a.HaveDocuments)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(a => a.CreatedAt)
               .HasColumnType("timestamptz")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(a => a.UpdatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasIndex(a => a.BreedId);
            builder.HasIndex(a => a.ShelterId);
        }
    }
}

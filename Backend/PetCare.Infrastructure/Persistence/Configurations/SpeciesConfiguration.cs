using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Domain.Entities;
using PetCare.Domain.ValueObjects;


namespace PetCare.Infrastructure.Persistence.Configurations
{
    public class SpeciesConfiguration : IEntityTypeConfiguration<Specie>
    {
        public void Configure(EntityTypeBuilder<Specie> builder)
        {
            builder.ToTable("Species");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(s => s.Name)
                .HasConversion(
                name => name.Value,
                value => Name.Create(value))
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(s => s.Name)
                .IsUnique();
        }
    }
}

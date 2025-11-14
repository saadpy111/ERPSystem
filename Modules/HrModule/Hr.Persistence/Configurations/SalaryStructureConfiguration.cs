using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class SalaryStructureConfiguration : IEntityTypeConfiguration<SalaryStructure>
    {
        public void Configure(EntityTypeBuilder<SalaryStructure> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.Code)
                .HasMaxLength(50);

            builder.Property(s => s.Description)
                .HasMaxLength(500);

            builder.Property(s => s.Type)
                .IsRequired()
                .HasConversion<string>();

            // Configure relationship with SalaryStructureComponent
            builder.HasMany(s => s.Components)
                .WithOne(c => c.SalaryStructure)
                .HasForeignKey(c => c.SalaryStructureId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
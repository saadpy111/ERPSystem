using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class SalaryStructureComponentConfiguration : IEntityTypeConfiguration<SalaryStructureComponent>
    {
        public void Configure(EntityTypeBuilder<SalaryStructureComponent> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Type)
                .IsRequired()
                .HasConversion<string>();

            // Configure relationship with SalaryStructure
            builder.HasOne(c => c.SalaryStructure)
                .WithMany(s => s.Components)
                .HasForeignKey(c => c.SalaryStructureId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
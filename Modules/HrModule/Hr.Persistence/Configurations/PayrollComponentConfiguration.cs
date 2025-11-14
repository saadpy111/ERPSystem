using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class PayrollComponentConfiguration : IEntityTypeConfiguration<PayrollComponent>
    {
        public void Configure(EntityTypeBuilder<PayrollComponent> builder)
        {
            builder.HasKey(pc => pc.ComponentId);

            builder.Property(pc => pc.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pc => pc.ComponentType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(pc => pc.FixedAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(pc => pc.PayrollRecord)
                .WithMany(pr => pr.Components)
                .HasForeignKey(pc => pc.PayrollRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

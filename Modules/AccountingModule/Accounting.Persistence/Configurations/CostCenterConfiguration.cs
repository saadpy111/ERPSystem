using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class CostCenterConfiguration : IEntityTypeConfiguration<CostCenter>
    {
        public void Configure(EntityTypeBuilder<CostCenter> builder)
        {
            builder.ToTable("CostCenters");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Code).HasMaxLength(50).IsRequired();
            builder.Property(e => e.NameAr).HasMaxLength(200).IsRequired();
            builder.Property(e => e.NameEn).HasMaxLength(200);

            builder.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
            builder.HasIndex(e => new { e.TenantId, e.Id });
            builder.HasIndex(e => e.ParentId);
        }
    }
}

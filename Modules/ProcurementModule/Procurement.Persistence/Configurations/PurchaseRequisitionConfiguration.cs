using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Persistence.Configurations
{
    public class PurchaseRequisitionConfiguration : IEntityTypeConfiguration<PurchaseRequisition>
    {
        public void Configure(EntityTypeBuilder<PurchaseRequisition> builder)
        {
            builder.HasKey(pr => pr.Id);
            
            builder.Property(pr => pr.RequestedBy)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(pr => pr.Status)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(pr => pr.Notes)
                .HasMaxLength(500);
        }
    }
}
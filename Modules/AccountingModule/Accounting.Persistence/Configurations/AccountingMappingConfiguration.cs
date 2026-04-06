using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class AccountingMappingConfiguration : IEntityTypeConfiguration<AccountingMapping>
    {
        public void Configure(EntityTypeBuilder<AccountingMapping> builder)
        {
            builder.ToTable("AccountingMappings");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.MappingKey).HasMaxLength(100).IsRequired();

            builder.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.TenantId, e.SourceType, e.MappingKey }).IsUnique();
            builder.HasIndex(e => e.TenantId);


        }
    }
}

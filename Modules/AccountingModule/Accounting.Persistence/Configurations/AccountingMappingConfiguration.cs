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

            // Seed Data
            builder.HasData(
                new AccountingMapping { Id = 1, TenantId = 1, SourceType = Domain.Enums.SourceType.Purchases, MappingKey = "Inventory", AccountId = 1, IsActive = true },
                new AccountingMapping { Id = 2, TenantId = 1, SourceType = Domain.Enums.SourceType.Purchases, MappingKey = "AccountsPayable", AccountId = 2, IsActive = true },
                new AccountingMapping { Id = 3, TenantId = 1, SourceType = Domain.Enums.SourceType.Inventory, MappingKey = "COGS", AccountId = 3, IsActive = true },
                new AccountingMapping { Id = 4, TenantId = 1, SourceType = Domain.Enums.SourceType.Inventory, MappingKey = "Inventory", AccountId = 1, IsActive = true },
                new AccountingMapping { Id = 5, TenantId = 1, SourceType = Domain.Enums.SourceType.Sales, MappingKey = "Cash", AccountId = 4, IsActive = true },
                new AccountingMapping { Id = 6, TenantId = 1, SourceType = Domain.Enums.SourceType.Sales, MappingKey = "Revenue", AccountId = 5, IsActive = true }
            );
        }
    }
}

using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Code).HasMaxLength(50).IsRequired();
            builder.Property(e => e.NameAr).HasMaxLength(200).IsRequired();
            builder.Property(e => e.NameEn).HasMaxLength(200);

            // Self-referencing relationship
            builder.HasOne(e => e.ParentAccount)
                .WithMany(e => e.ChildAccounts)
                .HasForeignKey(e => e.ParentAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => new { e.TenantId, e.Id });
            builder.HasIndex(e => e.ParentAccountId);
            builder.HasIndex(e => e.CurrencyId);

            // Soft delete query filter (applied globally in DbContext usually, but defined here if needed)
        }
    }
}

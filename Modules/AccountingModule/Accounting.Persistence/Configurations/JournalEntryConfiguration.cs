using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
    {
        public void Configure(EntityTypeBuilder<JournalEntry> builder)
        {
            builder.ToTable("JournalEntries");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.JournalNumber).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Reference).HasMaxLength(100);
            builder.Property(e => e.TotalDebit).HasColumnType("decimal(18,6)");
            builder.Property(e => e.TotalCredit).HasColumnType("decimal(18,6)");

            builder.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.FiscalPeriod)
                .WithMany(e => e.JournalEntries)
                .HasForeignKey(e => e.FiscalPeriodId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => new { e.TenantId, e.JournalNumber }).IsUnique();
            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => new { e.TenantId, e.Id });
            builder.HasIndex(e => e.Date);
            builder.HasIndex(e => e.CurrencyId);
            builder.HasIndex(e => e.FiscalPeriodId);
        }
    }
}

using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class JournalEntryLineConfiguration : IEntityTypeConfiguration<JournalEntryLine>
    {
        public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
        {
            builder.ToTable("JournalEntryLines");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Debit).HasColumnType("decimal(18,6)");
            builder.Property(e => e.Credit).HasColumnType("decimal(18,6)");
            builder.Property(e => e.ForeignAmount).HasColumnType("decimal(18,6)");
            builder.Property(e => e.ExchangeRate).HasColumnType("decimal(18,6)");
            builder.Property(e => e.BaseAmount).HasColumnType("decimal(18,6)");

            builder.HasOne(e => e.JournalEntry)
                .WithMany(e => e.Lines)
                .HasForeignKey(e => e.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Account)
                .WithMany(e => e.JournalEntryLines)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Partner)
                .WithMany(e => e.JournalEntryLines)
                .HasForeignKey(e => e.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CostCenter)
                .WithMany(e => e.JournalEntryLines)
                .HasForeignKey(e => e.CostCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.JournalEntryId);
            builder.HasIndex(e => e.AccountId);
            builder.HasIndex(e => e.PartnerId);
            builder.HasIndex(e => e.CostCenterId);
            builder.HasIndex(e => e.CurrencyId);
            builder.HasIndex(e => e.TenantId);
        }
    }
}

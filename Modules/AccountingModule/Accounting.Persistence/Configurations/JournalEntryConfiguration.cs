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

            // ── Reversal tracking columns ──────────────────────────────────────
            // Unidirectional model: only the reversal entry holds ReversedSourceJournalId.
            // The original journal is NEVER mutated after posting (immutable ledger rule).
            builder.Property(e => e.IsReversed).HasDefaultValue(false);
            builder.Property(e => e.ReversedAt).IsRequired(false);
            builder.Property(e => e.ReversedBy).HasMaxLength(256).IsRequired(false);
            builder.Property(e => e.ReversalReason).HasMaxLength(500).IsRequired(false);

            // ── Audit trail ────────────────────────────────────────────────────
            builder.Property(e => e.PostedBy).HasMaxLength(256).IsRequired(false);
            builder.Property(e => e.ApprovedBy).HasMaxLength(256).IsRequired(false);

            // ── Relationships ──────────────────────────────────────────────────
            builder.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.FiscalPeriod)
                .WithMany(e => e.JournalEntries)
                .HasForeignKey(e => e.FiscalPeriodId)
                .OnDelete(DeleteBehavior.Restrict);

            // Self-referencing FK: reversal journal → original journal (unidirectional)
            // ReversedSourceJournalId lives ONLY on the reversal entry.
            // The original entry has NO back-reference (immutable after posting).
            builder.HasOne(e => e.ReversedSourceJournal)
                .WithMany(e => e.Reversals)
                .HasForeignKey(e => e.ReversedSourceJournalId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // ── Indexes ────────────────────────────────────────────────────────
            builder.HasIndex(e => new { e.TenantId, e.JournalNumber }).IsUnique();
         //   builder.HasIndex(e => new { e.TenantId, e.SourceType, e.SourceId }).IsUnique();
            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => new { e.TenantId, e.Id });
            builder.HasIndex(e => e.Date);
            builder.HasIndex(e => e.CurrencyId);
            builder.HasIndex(e => e.FiscalPeriodId);
            builder.HasIndex(e => e.ReversedSourceJournalId);  // FK look-ups for reversal chains
        }
    }
}

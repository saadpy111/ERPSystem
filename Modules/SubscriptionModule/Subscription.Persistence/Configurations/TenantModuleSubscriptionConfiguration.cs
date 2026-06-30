using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription.Domain.Entities;

namespace Subscription.Persistence.Configurations
{
    public class TenantModuleSubscriptionConfiguration : IEntityTypeConfiguration<TenantModuleSubscription>
    {
        public void Configure(EntityTypeBuilder<TenantModuleSubscription> builder)
        {
            builder.ToTable("TenantModuleSubscriptions", "Subscription");

            builder.HasKey(tms => tms.Id);

            builder.Property(tms => tms.CurrencyCode)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(tms => tms.ExternalSubscriptionId)
                .HasMaxLength(100);

            builder.Property(tms => tms.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(tms => tms.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasIndex(tms => tms.TenantId);
            builder.HasIndex(tms => tms.Status);
            builder.HasIndex(tms => new { tms.TenantId, tms.Status });

            builder.HasOne(tms => tms.Module)
                .WithMany()
                .HasForeignKey(tms => tms.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

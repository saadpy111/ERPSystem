using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription.Domain.Entities;

namespace Subscription.Persistence.Configurations
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.ToTable("Modules", "Subscription");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.DisplayName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.Description)
                .HasMaxLength(500);

            builder.HasIndex(m => m.Code)
                .IsUnique();

            builder.HasIndex(m => m.IsActive);
        }
    }
}

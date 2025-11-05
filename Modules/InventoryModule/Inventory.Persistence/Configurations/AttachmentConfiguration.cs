using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Inventory.Domain.Entities.Attachment>
    {
        public void Configure(EntityTypeBuilder<Inventory.Domain.Entities.Attachment> builder)
        {
            builder.ToTable("Attachments");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.FileName).HasMaxLength(255).IsRequired();
            builder.Property(a => a.FileUrl).HasMaxLength(1024).IsRequired();
            builder.Property(a => a.ContentType).HasMaxLength(128);
            builder.Property(a => a.EntityType).HasMaxLength(128).IsRequired();
            builder.Property(a => a.Description).HasMaxLength(512);
            builder.Property(a => a.UploadedAt).IsRequired();
            builder.HasIndex(a => new { a.EntityType, a.EntityId }); // For fast lookup
        }
    }
}

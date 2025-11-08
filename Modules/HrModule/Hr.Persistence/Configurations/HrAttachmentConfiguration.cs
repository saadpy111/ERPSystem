using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class HrAttachmentConfiguration : IEntityTypeConfiguration<HrAttachment>
    {
        public void Configure(EntityTypeBuilder<HrAttachment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.FileUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.ContentType)
                .HasMaxLength(100);

            builder.Property(a => a.EntityType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Description)
                .HasMaxLength(500);

            // Configure the polymorphic relationship
            builder.HasIndex(a => new { a.EntityType, a.EntityId });
        }
    }
}
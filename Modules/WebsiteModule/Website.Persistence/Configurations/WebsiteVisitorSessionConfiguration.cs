using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    public class WebsiteVisitorSessionConfiguration : IEntityTypeConfiguration<WebsiteVisitorSession>
    {
        public void Configure(EntityTypeBuilder<WebsiteVisitorSession> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.SessionId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.UserId)
                .HasMaxLength(50);

            builder.Property(x => x.IpAddress)
                .HasMaxLength(50);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(500);

            builder.HasIndex(x => x.SessionId);

            builder.HasIndex(x => new { x.TenantId, x.SessionId }).IsUnique();
        }
    }
}

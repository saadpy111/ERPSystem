using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    public class NewsletterSubscriberConfiguration : IEntityTypeConfiguration<NewsletterSubscriber>
    {
        public void Configure(EntityTypeBuilder<NewsletterSubscriber> builder)
        {
            builder.ToTable("NewsletterSubscribers", "Website");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(n => n.TenantId)
                .IsRequired();

            builder.Property(n => n.SubscribedAt)
                .IsRequired();

            builder.HasIndex(n => new { n.TenantId, n.Email })
                .IsUnique();
        }
    }
}

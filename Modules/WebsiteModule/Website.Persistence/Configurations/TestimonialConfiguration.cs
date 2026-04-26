using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    public class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
    {
        public void Configure(EntityTypeBuilder<Testimonial> builder)
        {
            builder.ToTable("Testimonials");

            builder.Property(t => t.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Comment)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(t => t.Rating)
                .IsRequired();

            // Index according to requirements: (TenantId, Order)
            builder.HasIndex(t => new { t.TenantId, t.Order });
        }
    }
}

using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.DepartmentId);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(d => d.Jobs)
                .WithOne(j => j.Department)
                .HasForeignKey(j => j.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

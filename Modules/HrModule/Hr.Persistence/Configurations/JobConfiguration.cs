using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasKey(j => j.JobId);

            builder.Property(j => j.Title)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(j => j.Responsibilities)
                .HasMaxLength(2000);

            builder.Property(j => j.RequiredSkills)
                .HasMaxLength(2000);

            builder.Property(j => j.RequiredExperience)
                .HasMaxLength(2000);

            builder.Property(j => j.RequiredQualification)
                .HasMaxLength(2000);

            builder.Property(j => j.WorkType)
                .HasConversion<string>();

            builder.Property(j => j.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasOne(j => j.Department)
                .WithMany(d => d.Jobs)
                .HasForeignKey(j => j.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(j => j.Applicants)
                .WithOne(a => a.AppliedJob)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
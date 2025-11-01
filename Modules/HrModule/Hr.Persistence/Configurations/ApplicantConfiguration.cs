using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
    {
        public void Configure(EntityTypeBuilder<Applicant> builder)
        {
            builder.HasKey(a => a.ApplicantId);

            builder.Property(a => a.FullName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(a => a.ResumeUrl)
                .HasMaxLength(255);

            builder.HasOne(a => a.AppliedJob)
                .WithMany(j => j.Applicants)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.CurrentStage)
                .WithMany()
                .HasForeignKey(a => a.CurrentStageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

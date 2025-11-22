using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class ApplicantEducationConfiguration : IEntityTypeConfiguration<ApplicantEducation>
    {
        public void Configure(EntityTypeBuilder<ApplicantEducation> builder)
        {
            builder.HasKey(ae => ae.Id);

            builder.Property(ae => ae.DegreeName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(ae => ae.Specialization)
                .HasMaxLength(200);

            builder.Property(ae => ae.Institute)
                .HasMaxLength(300);

            builder.Property(ae => ae.Grade)
                .HasMaxLength(50);

            // Configure the relationship with Applicant
            builder.HasOne(ae => ae.Applicant)
                .WithMany(a => a.Educations)
                .HasForeignKey(ae => ae.ApplicantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
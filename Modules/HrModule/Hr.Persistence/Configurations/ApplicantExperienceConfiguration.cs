using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class ApplicantExperienceConfiguration : IEntityTypeConfiguration<ApplicantExperience>
    {
        public void Configure(EntityTypeBuilder<ApplicantExperience> builder)
        {
            builder.HasKey(ae => ae.Id);

            builder.Property(ae => ae.CompanyName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(ae => ae.JobTitle)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(ae => ae.Description)
                .HasMaxLength(1000);

            // Configure the relationship with Applicant
            builder.HasOne(ae => ae.Applicant)
                .WithMany(a => a.Experiences)
                .HasForeignKey(ae => ae.ApplicantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
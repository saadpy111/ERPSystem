using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class RecruitmentStageConfiguration : IEntityTypeConfiguration<RecruitmentStage>
    {
        public void Configure(EntityTypeBuilder<RecruitmentStage> builder)
        {
            builder.HasKey(rs => rs.StageId);

            builder.Property(rs => rs.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(rs => rs.SequenceOrder)
                .IsRequired();

            builder.Property(rs => rs.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}

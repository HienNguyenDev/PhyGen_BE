using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.ExamSets;

namespace PhyGen.Infrastructure.Configurations
{
    public class ExamSetQuestionConfiguration : IEntityTypeConfiguration<ExamSetQuestion>
    {
        public void Configure(EntityTypeBuilder<ExamSetQuestion> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Order);

            builder.HasOne(x => x.ExamSet)
                   .WithMany(x => x.ExamSetQuestions)
                   .HasForeignKey(x => x.ExamSetId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Question)
                   .WithMany(x => x.ExamSetQuestions)
                   .HasForeignKey(x => x.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.ExamSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                   .HasForeignKey(x => x.ExamSetId);

            builder.HasOne(x => x.Question)
                   .WithMany()
                   .HasForeignKey(x => x.QuestionId);
        }
    }

}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.QuestionText).HasColumnType("text");
            builder.Property(x => x.SourceFile).HasMaxLength(255);
            builder.Property(x => x.CreatedAt);
            builder.Property(x => x.AIConfidence);

            builder.HasOne(x => x.Class)
                   .WithMany(x => x.Questions)
                   .HasForeignKey(x => x.ClassId);

            builder.HasOne(x => x.Chapter)
                   .WithMany(x => x.Questions)
                   .HasForeignKey(x => x.ChapterId);

            builder.HasOne(x => x.Type)
                   .WithMany(x => x.Questions)
                   .HasForeignKey(x => x.TypeId);
        }
    }

}

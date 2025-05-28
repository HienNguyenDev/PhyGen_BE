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
    public class QuestionTemplateConfiguration : IEntityTypeConfiguration<QuestionTemplate>
    {
        public void Configure(EntityTypeBuilder<QuestionTemplate> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TemplateText).HasColumnType("text");

            builder.HasOne(x => x.Chapter)
                   .WithMany(x => x.QuestionTemplates)
                   .HasForeignKey(x => x.ChapterId);

            builder.HasOne(x => x.Type)
                   .WithMany(x => x.QuestionTemplates)
                   .HasForeignKey(x => x.TypeId);
        }
    }

}

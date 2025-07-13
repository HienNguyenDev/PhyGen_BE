using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Questions;

namespace PhyGen.Infrastructure.Configurations
{
    public class QuestionTemplateConfiguration : IEntityTypeConfiguration<QuestionTemplate>
    {
        public void Configure(EntityTypeBuilder<QuestionTemplate> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TemplateText).IsRequired().HasColumnType("text");

            builder.HasOne(x => x.Chapter)
                   .WithMany(x => x.QuestionTemplates)
                   .HasForeignKey(x => x.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

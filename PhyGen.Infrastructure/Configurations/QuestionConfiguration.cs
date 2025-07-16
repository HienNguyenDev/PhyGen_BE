using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Questions;

namespace PhyGen.Infrastructure.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.QuestionContent).IsRequired().HasColumnType("text");
            builder.Property(x => x.Difficulty);
            builder.Property(x => x.CreatedAt);

            builder.HasOne(x => x.Class)
                   .WithMany(x => x.Questions)
                   .HasForeignKey(x => x.ClassId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Chapter)
                   .WithMany(x => x.Questions)
                   .HasForeignKey(x => x.ChapterId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Topic)
                   .WithMany()
                   .HasForeignKey(x => x.TopicId)
                   .OnDelete(DeleteBehavior.SetNull);

        }
    }
}

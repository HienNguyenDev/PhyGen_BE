using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Chapters;

namespace PhyGen.Infrastructure.Configurations
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ChapterName).IsRequired().HasMaxLength(255);

            builder.HasOne(x => x.Class)
                   .WithMany(x => x.Chapters)
                   .HasForeignKey(x => x.ClassId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Topics)
                   .WithOne(x => x.Chapter)
                   .HasForeignKey(x => x.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Questions)
                   .WithOne(x => x.Chapter)
                   .HasForeignKey(x => x.ChapterId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.QuestionTemplates)
                   .WithOne(x => x.Chapter)
                   .HasForeignKey(x => x.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

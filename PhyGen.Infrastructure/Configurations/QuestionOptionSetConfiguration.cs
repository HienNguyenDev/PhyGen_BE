using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Questions;

namespace PhyGen.Infrastructure.Configurations
{
    public class QuestionOptionSetConfiguration : IEntityTypeConfiguration<QuestionOptionSet>
    {
        public void Configure(EntityTypeBuilder<QuestionOptionSet> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.A).IsRequired().HasMaxLength(255);
            builder.Property(x => x.B).IsRequired().HasMaxLength(255);
            builder.Property(x => x.C).IsRequired().HasMaxLength(255);
            builder.Property(x => x.D).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Correct).HasMaxLength(10);

            builder.HasOne(x => x.Question)
                   .WithOne(x => x.QuestionOptionSet)
                   .HasForeignKey<QuestionOptionSet>(x => x.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 
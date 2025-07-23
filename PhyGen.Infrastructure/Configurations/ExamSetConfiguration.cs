using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.ExamSets;

namespace PhyGen.Infrastructure.Configurations
{
    public class ExamSetConfiguration : IEntityTypeConfiguration<ExamSet>
    {
        public void Configure(EntityTypeBuilder<ExamSet> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Description).HasColumnType("text");
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedAt);

            builder.HasOne(x => x.Class)
                   .WithMany(x => x.ExamSets)
                   .HasForeignKey(x => x.ClassId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Creator)
                   .WithMany(u => u.CreatedExamSets)
                   .HasForeignKey(x => x.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

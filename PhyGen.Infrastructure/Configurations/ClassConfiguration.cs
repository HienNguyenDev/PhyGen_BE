using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Classes;

namespace PhyGen.Infrastructure.Configurations
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ClassName).IsRequired().HasMaxLength(255);

            builder.HasMany(x => x.Chapters)
                   .WithOne(x => x.Class)
                   .HasForeignKey(x => x.ClassId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Questions)
                   .WithOne(x => x.Class)
                   .HasForeignKey(x => x.ClassId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.ExamSets)
                   .WithOne(x => x.Class)
                   .HasForeignKey(x => x.ClassId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


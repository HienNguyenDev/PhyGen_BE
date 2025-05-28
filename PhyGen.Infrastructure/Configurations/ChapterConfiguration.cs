using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Chapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Configurations
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ChapterName).HasMaxLength(255);
            builder.Property(x => x.ChapterCode).HasMaxLength(100);

            builder.HasOne(x => x.Class)
                   .WithMany(x => x.Chapters)
                   .HasForeignKey(x => x.ClassId);
        }
    }

}

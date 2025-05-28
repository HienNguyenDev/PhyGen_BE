using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Configurations
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Keyword).HasMaxLength(255);
            builder.Property(x => x.Synonyms).HasColumnType("text");

            builder.HasOne(x => x.Chapter)
                   .WithMany(x => x.Topics)
                   .HasForeignKey(x => x.ChapterId);
        }
    }

}

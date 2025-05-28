using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.ExamSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            builder.Property(x => x.ReviewNote).HasColumnType("text");

            builder.HasOne(x => x.Class)
                   .WithMany(x => x.ExamSets)
                   .HasForeignKey(x => x.ClassId);

            builder.HasOne(x => x.Creator)
                   .WithMany()
                   .HasForeignKey(x => x.CreatedBy);

            builder.HasOne(x => x.Reviewer)
                   .WithMany()
                   .HasForeignKey(x => x.ReviewedBy);
        }
    }

}

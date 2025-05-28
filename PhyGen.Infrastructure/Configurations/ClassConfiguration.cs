using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Configurations
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ClassName).HasMaxLength(255);
            builder.Property(x => x.Description).HasColumnType("text");

            builder.HasMany(x => x.Chapters)
                   .WithOne(x => x.Class)
                   .HasForeignKey(x => x.ClassId);
        }
    }
}

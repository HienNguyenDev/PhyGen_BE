using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Topics;

namespace PhyGen.Infrastructure.Configurations
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TopicName).IsRequired().HasMaxLength(255);

            builder.HasOne(x => x.Chapter)
                   .WithMany(x => x.Topics)
                   .HasForeignKey(x => x.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

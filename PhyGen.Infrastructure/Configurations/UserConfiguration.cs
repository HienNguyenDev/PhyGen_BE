using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Users;

namespace PhyGen.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Username).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.IdentityId).IsRequired();
            builder.Property(x => x.IsProvider).IsRequired();
            builder.Property(x => x.Role).IsRequired().HasConversion<int>();
            builder.Property(x => x.Status).IsRequired().HasConversion<int>();
            builder.Property(x => x.CreatedAt);

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.IdentityId).IsUnique();

            builder.HasMany(x => x.CreatedExamSets)
                   .WithOne(x => x.Creator)
                   .HasForeignKey(x => x.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ReviewedExamSets)
                   .WithOne(x => x.Reviewer)
                   .HasForeignKey(x => x.ReviewedBy)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

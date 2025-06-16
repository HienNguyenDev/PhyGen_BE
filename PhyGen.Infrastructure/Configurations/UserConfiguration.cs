using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedNever(); 

            builder.Property(x => x.Username).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Role).IsRequired().HasConversion<int>(); 
            builder.Property(x => x.CreatedAt);
            builder.Property(x => x.Status).IsRequired().HasConversion<int>().HasDefaultValue(UserStatus.Active);
            builder.Property(x => x.IsProvider)
            .IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.IdentityId).IsUnique();

            builder.HasMany(x => x.CreatedExamSets)
                .WithOne(es => es.Creator)
                .HasForeignKey(es => es.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ReviewedExamSets)
                .WithOne(es => es.Reviewer)
                .HasForeignKey(es => es.ReviewedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}

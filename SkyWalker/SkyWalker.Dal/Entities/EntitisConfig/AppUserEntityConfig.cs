using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyWalker.Dal.Entities 
{
    public class AppUserEntityConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUser")
                 .HasKey(x => new { x.Id, x.UserId });
            builder.Property(x => x.UserId).HasColumnType("varchar(50)");
            builder.Property(x => x.UserName).HasColumnType("varchar(50)");
            builder.Property(x => x.UserPassWord).HasColumnType("varchar(50)");
            builder.Property(x => x.Avatar).HasColumnType("varchar(200)");
            builder.Property(x => x.Phone).HasColumnType("varchar(20)");
            builder.Property(x => x.Describe).HasColumnType("varchar(500)");
            builder.Property(x => x.Brithday).HasColumnType("datetime");
        }
    }
}

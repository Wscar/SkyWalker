using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyWalker.Dal.Entities
{
    public class StoryEntityConfig : IEntityTypeConfiguration<Story>
    {
        public void Configure(EntityTypeBuilder<Story> builder)
        {
            builder.ToTable("story");
            builder.Property(x => x.Content).HasColumnName("content").HasColumnType("varchar(1000)");
            builder.Property(x => x.CoverPhoto).HasColumnName("conver_photo").HasColumnType("varchar(200)");
            builder.Property(x => x.Title).HasColumnName("title").HasColumnType("varchar(200)");
            builder.HasKey(x => x.Id);
        }
    }
}

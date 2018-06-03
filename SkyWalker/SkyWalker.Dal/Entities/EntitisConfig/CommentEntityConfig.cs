using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyWalker.Dal.Entities
{
    public class CommentEntityConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comment");
            builder.Property(x => x.Content).HasColumnName("content").HasColumnType("text");
            builder.HasKey(x => new { x.Id});           
        }
    }
}

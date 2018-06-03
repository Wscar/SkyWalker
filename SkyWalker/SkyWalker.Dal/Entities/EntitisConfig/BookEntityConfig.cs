using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyWalker.Dal.Entities
{
    public class BookEntityConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("book");
            builder.Property(x => x.BookId).HasColumnName("book_id");
            builder.Property(x => x.BookName).HasColumnName("book_name").HasColumnType("varchar(100)");
            builder.Property(x => x.Preface).HasColumnName("preface").HasColumnType("varchar(500)");
            builder.Property(x => x.Status).HasColumnName("status").HasColumnType("varchar(100)");
            builder.HasKey(x => x.BookId);
        }
    }
}

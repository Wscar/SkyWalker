using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkyWalker.Dal.Entities;
namespace SkyWalker.Dal.DBContext
{
    public class SkyWalkerDbContext:DbContext
    {
        public SkyWalkerDbContext(DbContextOptions<SkyWalkerDbContext> options):base(options)
        {
            
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppUserEntityConfig());
            modelBuilder.ApplyConfiguration(new BookEntityConfig());
            modelBuilder.ApplyConfiguration(new CommentEntityConfig());
            modelBuilder.ApplyConfiguration(new StoryEntityConfig());
        }
    }
}

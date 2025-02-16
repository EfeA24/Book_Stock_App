using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.EfCore.Config;

namespace Repositories.EfCore
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Books> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfig());
        }
    }
}


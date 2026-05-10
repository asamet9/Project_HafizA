using Microsoft.EntityFrameworkCore;
using Project_HafizA.Core;

namespace Project_HafizA.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Main> Main { get; set; }
        public DbSet<Liderlik> Liderlik { get; set; }
    }
}
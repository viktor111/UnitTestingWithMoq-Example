using Microsoft.EntityFrameworkCore;
using ProductsCatalog.Models;

namespace ProductsCatalog.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}

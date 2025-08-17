using Microsoft.EntityFrameworkCore;
using ProductService.Core.Models;

namespace ProductService.Core
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Console.WriteLine("AppDbContext ctor");
        }
    }
}

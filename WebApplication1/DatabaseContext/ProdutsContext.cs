using LearningPathDev.Models;
using Microsoft.EntityFrameworkCore;
namespace LearningPathDev.DatabaseContext
{
    public class ProductsContext : DbContext
    {

        public ProductsContext(DbContextOptions<ProductsContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        public DbSet<Product> Product { get; set; }
    }
}

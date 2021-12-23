using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AppStory.DataBase
{
    public class StoryDbContext : DbContext, IOrdersDbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public StoryDbContext(DbContextOptions option) : base(option)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Product.EntityConfiguration());
            modelBuilder.ApplyConfiguration(new Order.EntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItem.EntityConfiguration());
        }
    }

    public interface IDbContextBase
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public interface IProductsDbContext : IDbContextBase
    {
        DbSet<Product> Products { get; set; }
    }

    public interface IOrdersDbContext : IProductsDbContext
    {
        DbSet<Order> Orders { get; set; }

        DbSet<OrderItem> OrderItems { get; set; }
    }
}

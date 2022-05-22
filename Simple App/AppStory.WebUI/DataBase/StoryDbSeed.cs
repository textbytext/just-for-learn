using System.Linq;
using System.Threading.Tasks;

namespace AppStory.DataBase
{
    public static class StoryDbSeed
    {
        public static void Seed(StoryDbContext context)
        {
            if (context.Products.Any())
            {
                return;
            }

            // add products
            Enumerable.Range(1, 100)
                .ToList()
                .ForEach(i =>
                {
                    context.Products.Add(new Product
                    {
                        Name = $"Product name {i}",
                    });
                });

            context.SaveChanges();

            // add orders
            var order = new Order
            {
                Name = $"Order name 1",
            };
            order.OrderItems.Add(new OrderItem
            {
                ProductId = 1,
            });
            order.OrderItems.Add(new OrderItem
            {
                ProductId = 2,
            });

            context.Orders.Add(order);

            context.SaveChanges();
        }
    }
}

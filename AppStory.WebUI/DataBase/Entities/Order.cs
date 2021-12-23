using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace AppStory.DataBase
{
    public class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public class EntityConfiguration : IEntityTypeConfiguration<Order>
        {
            public void Configure(EntityTypeBuilder<Order> builder)
            {
                builder.HasKey(i => i.Id);
            }
        }
    }
}

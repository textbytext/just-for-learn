using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace AppStory.DataBase
{
    public class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public class EntityConfiguration : IEntityTypeConfiguration<Product>
        {
            public void Configure(EntityTypeBuilder<Product> builder)
            {
                builder.HasKey(i => i.Id);
            }
        }
    }
}

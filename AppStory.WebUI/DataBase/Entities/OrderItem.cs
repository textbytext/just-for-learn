using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppStory.DataBase
{
    public class OrderItem
    {
        public long Id { get; set; }

        public long OrderId { get; set; }

        public virtual Order Order { get; set; }

        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        public class EntityConfiguration : IEntityTypeConfiguration<OrderItem>
        {
            public void Configure(EntityTypeBuilder<OrderItem> builder)
            {
                builder.HasKey(i => i.Id);

                builder.HasOne(i => i.Order)
                    .WithMany(i => i.OrderItems)
                    .HasForeignKey(i => i.OrderId);

                builder.HasOne(i => i.Product)
                    .WithMany(i => i.OrderItems)
                    .HasForeignKey(i => i.ProductId);
            }
        }
    }
}

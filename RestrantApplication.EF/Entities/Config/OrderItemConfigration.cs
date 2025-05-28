using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestrantApplication.Core.Models.Order;

namespace RestrantApplication.EF.Entities.Config
{
    public class OrderItemConfigration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(oi => oi.Order)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(oi => oi.OrderID);

            builder.HasOne(oi => oi.Product)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(oi => oi.ProductID);
        }
    }
}

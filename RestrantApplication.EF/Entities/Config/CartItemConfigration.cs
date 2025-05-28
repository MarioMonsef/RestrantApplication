using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestrantApplication.Core.Models.Cart;

namespace RestrantApplication.EF.Entities.Config
{
    public class CartItemConfigration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasOne(ci => ci.Cart)
                .WithMany(ci => ci.CartItems)
                .HasForeignKey(ci => ci.CartID);

            builder.HasOne(ci => ci.Product)
                .WithMany(ci => ci.CartItems)
                .HasForeignKey(ci => ci.ProductID);
        }
    }
}

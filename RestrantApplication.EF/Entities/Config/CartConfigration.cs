using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestrantApplication.Core.Models.Cart;

namespace RestrantApplication.EF.Entities.Config
{
    public class CartConfigration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasOne(c => c.ApplictionUser)
                .WithOne(c => c.Cart)
                .HasForeignKey<Cart>(c => c.UserID);


        }
    }
}

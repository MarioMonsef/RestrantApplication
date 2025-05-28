using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestrantApplication.Core.Models.Order;

namespace RestrantApplication.EF.Entities.Config
{
    public class OrderConfigration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(x => x.ApplictionUser)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserID);

        }
    }
}

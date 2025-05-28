using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestrantApplication.Core.Models.Product;

namespace RestrantApplication.EF.Entities.Config
{
    public class ProductConfigration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasOne(x => x.category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.categoryID);

            builder.HasOne(x => x.Photo)
                .WithOne(x => x.Product)
                .HasForeignKey<Product>(x => x.PhotoID);
        }
    }
}

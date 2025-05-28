using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestrantApplication.Core.Models.Review;

namespace RestrantApplication.EF.Entities.Config
{
    public class ReviewConfigration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasOne(x => x.ApplictionUser)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.UserID);


        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestrantApplication.Core.Models.Identity;

namespace RestrantApplication.EF.Entities.Config
{
    public class ApplicationUserConfigration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne(a => a.UserPicture)
                .WithOne(p => p.User)
                .HasForeignKey<ApplicationUser>(f => f.PictureID);
        }
    }
}

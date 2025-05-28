using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestrantApplication.Core.Models.Cart;
using RestrantApplication.Core.Models.Identity;
using RestrantApplication.Core.Models.Order;
using RestrantApplication.Core.Models.Product;
using RestrantApplication.Core.Models.Review;
using System.Reflection;

namespace RestrantApplication.EF.Entities
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Category>  Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual  DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(assembly: Assembly.GetExecutingAssembly());

        }
    }
}

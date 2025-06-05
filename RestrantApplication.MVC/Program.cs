using Microsoft.AspNetCore.Identity;
using RestrantApplication.Core.Models.Identity;
using RestrantApplication.EF;
using RestrantApplication.EF.Entities;

namespace RestrantApplication.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMemoryCache();
            // Add services to the container.
            builder.Services.AddControllersWithViews(Options => {
                Options.Filters.Add<SecurityHeadersFilter>();
                Options.Filters.Add<RateLimitFilter>();
                });

            builder.Services.infrastractureConfigration(builder.Configuration);
            builder.Services.AddIdentity<ApplicationUser,IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Home}/{id?}");

            app.Run();
        }
    }
}

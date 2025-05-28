using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Services;
using RestrantApplication.EF.Entities;
using RestrantApplication.EF.Repository;
using RestrantApplication.EF.Services;
using StackExchange.Redis;

namespace RestrantApplication.EF
{
    public static class InfrastractureRegistration
    {
        public static IServiceCollection infrastractureConfigration(this IServiceCollection services,IConfiguration configration) {

            //apply DBContext
            services.AddDbContext<AppDBContext>(option =>
            option.UseSqlServer(configration.GetConnectionString("RestrantDB"))
            );
            //apply unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IReviewService, ReviewService>();

            services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(configration["Redis:ConnectionString"]));
            services.AddSingleton<IRedisService,RedisService>();

            return services;
        }
    }
}

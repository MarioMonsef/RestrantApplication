using AutoMapper;
using RestrantApplication.Core.Models.Identity;
using RestrantApplication.Core.Models.Order;
using RestrantApplication.Core.ViewModels.Order;

namespace RestrantApplication.EF.Mapping
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            CreateMap<Order,OrderViewModel>().ReverseMap();
            CreateMap<OrderItem, OrderItemsViewModel>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserOrderViewModel>().ReverseMap();
        }
    }
}

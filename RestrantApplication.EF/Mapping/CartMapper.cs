using AutoMapper;
using RestrantApplication.Core.Models.Cart;
using RestrantApplication.Core.Models.Product;
using RestrantApplication.Core.ViewModels.Cart;

namespace RestrantApplication.EF.Mapping
{
    public class CartMapper :Profile
    {
        public CartMapper()
        {
            CreateMap<CartViewModel,Cart>().ReverseMap();
            CreateMap<CartItemsViewModel, CartItem>().ReverseMap();
            CreateMap<ProductcartViewModel, Product>().ReverseMap();
            CreateMap<PhotoCartViewModel, Photo>().ReverseMap();
        }
    }
}

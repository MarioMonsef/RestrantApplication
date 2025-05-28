using AutoMapper;
using RestrantApplication.Core.Models.Product;
using RestrantApplication.Core.ViewModels.Product;

namespace RestrantApplication.EF.Mapping
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<AddProductAndImageViewModel, Product>().ReverseMap();
            CreateMap<UpdateProductViewModel, Product>()
                .ForMember(p=>p.PhotoID,p=>
                p.MapFrom(p=>p.OldPhotoID)).ReverseMap();

            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.category, opt => opt.MapFrom(src => src.category))
                .ReverseMap();

            CreateMap<Category, CategoryProductViewModel>().ReverseMap();
            CreateMap<Photo, PhotoProductViewModel>().ReverseMap();


        }
    }
}

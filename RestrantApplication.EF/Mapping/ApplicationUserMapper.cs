using AutoMapper;
using RestrantApplication.Core.Models.Identity;
using RestrantApplication.Core.ViewModels.Identity;

namespace RestrantApplication.EF.Mapping
{
    public class ApplicationUserMapper:Profile
    {
        public ApplicationUserMapper()
        {
            CreateMap<RegisterViewModel,ApplicationUser>().ReverseMap();
            CreateMap<RegisterByAdminViewModel,ApplicationUser>().ReverseMap();
        }
    }
}

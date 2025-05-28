using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Identity;
using RestrantApplication.EF.Entities;

namespace RestrantApplication.EF.Repository
{
    public class PictureRepository : GenericRepository<UserPicture>,IPictureRepository 
    {
        public PictureRepository(AppDBContext context) : base(context)
        {
        }
    }
}

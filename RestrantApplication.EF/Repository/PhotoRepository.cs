using RestrantApplication.Core.Interfaces;
using RestrantApplication.Core.Models.Product;
using RestrantApplication.EF.Entities;

namespace RestrantApplication.EF.Repository
{
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(AppDBContext context) : base(context)
        {
        }
    }
}

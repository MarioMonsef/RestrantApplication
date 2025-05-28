
using RestrantApplication.Core.Models.Product;
using RestrantApplication.Core.ViewModels.Product;
using RestrantApplication.Core.ViewModels.Review;

namespace RestrantApplication.Core.ViewModels
{
    public class HomeDataViewModel
    {
        public IReadOnlyList<ProductViewModel> Products { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? TotalCount { get; set; }
        public IReadOnlyList<ReviewViewModel> Reviews { get; set; }

    }
}

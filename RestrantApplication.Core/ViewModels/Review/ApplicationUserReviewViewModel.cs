using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Review
{
    public record ApplicationUserReviewViewModel
    {
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
        public virtual UserPictureReviewViewModel UserPicture { get; set; }
    }
}

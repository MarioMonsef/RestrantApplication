using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Review
{
    public class UpdateReviewViewModel
    {
        public int ID { get; set; }
        public string Comment { get; set; }
        public string UserID { get; set; }
    }
}

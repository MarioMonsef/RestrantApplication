using RestrantApplication.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestrantApplication.Core.ViewModels.Order;

namespace RestrantApplication.Core.ViewModels.Review
{
    public record ReviewViewModel
    {
        public int ID { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public virtual ApplicationUserReviewViewModel ApplictionUser { get; set; }
    }
}

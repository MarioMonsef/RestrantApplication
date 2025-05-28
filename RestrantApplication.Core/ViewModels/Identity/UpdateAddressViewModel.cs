using RestrantApplication.Core.Models.Order;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Identity
{
    public class UpdateAddressViewModel
    {
        [Required]
        public string Address { get; set; }
        [Required]
        [Range(1, 2)]
        [Display(Name = "Order Type")]
        public OrderType orderType { get; set; }
    }
}

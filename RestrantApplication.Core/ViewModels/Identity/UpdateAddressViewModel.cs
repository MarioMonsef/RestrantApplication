using RestrantApplication.Core.Models.Order;
using System.ComponentModel.DataAnnotations;

namespace RestrantApplication.Core.ViewModels.Identity
{
    public record UpdateAddressViewModel
    {
        [Required]
        public string Address { get; set; }
        [Required]
        [Range(1, 2)]
        [Display(Name = "Order Type")]
        public OrderType orderType { get; set; }
    }
}

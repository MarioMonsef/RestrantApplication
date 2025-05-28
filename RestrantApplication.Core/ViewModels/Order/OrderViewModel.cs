using RestrantApplication.Core.Models.Identity;
using RestrantApplication.Core.Models.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Order
{
    public record OrderViewModel
    {
        public int ID { get; set; }
        public OrderState orderState { get; set; }
        public OrderType orderType { get; set; }

        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public virtual ApplicationUserOrderViewModel ApplictionUser { get; set; }
        public virtual List<OrderItemsViewModel> OrderItems { get; set; }
    }
}

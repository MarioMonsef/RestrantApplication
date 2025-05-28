using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Order
{
    public record OrderItemsViewModel
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public decimal PriseAtOrder { get; set; }
        public decimal TotalAmount => Quantity * PriseAtOrder;

    }
}

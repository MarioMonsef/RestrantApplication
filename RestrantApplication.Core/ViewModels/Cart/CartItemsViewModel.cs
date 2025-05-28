using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Cart
{
    public class CartItemsViewModel
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public int ProductID { get; set; }
        public decimal TotalAmount => Quantity * Product.Prise;
        public virtual ProductcartViewModel Product { get; set; }
    }
}

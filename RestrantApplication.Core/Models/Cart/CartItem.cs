using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.Models.Cart
{
    public class CartItem : BaseModel<int>
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Can't Enter Negative Number.")]
        public int Quantity { get; set; }
        public int ProductID { get; set; }
        public int CartID { get; set; }
        public decimal TotalAmount => Quantity * Product.Prise;

        [ForeignKey("ProductID")]
        public virtual RestrantApplication.Core.Models.Product.Product Product { get; set; }
        [ForeignKey("CartID")]
        public virtual Cart Cart { get; set; }
    }
}

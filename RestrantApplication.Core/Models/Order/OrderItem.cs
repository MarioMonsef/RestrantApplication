
using RestrantApplication.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RestrantApplication.Core.Models.Order
{
    public class OrderItem : BaseModel<int>
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Can't Enter Negative Number.")]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriseAtOrder { get; set; }

        public int ProductID { get; set; }
        public int OrderID { get; set; }
        [ForeignKey("ProductID")]
        public virtual RestrantApplication.Core.Models.Product.Product Product { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        public decimal TotalAmount => Quantity * PriseAtOrder;
    }
}

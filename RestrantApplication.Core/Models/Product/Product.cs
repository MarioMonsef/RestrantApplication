using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestrantApplication.Core.Models;
using RestrantApplication.Core.Models.Cart;
using RestrantApplication.Core.Models.Order;

namespace RestrantApplication.Core.Models.Product
{
    public class Product : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Prise { get; set; }
        [Required]
        public decimal Stock { get; set; }
        [Required]
        public bool IsAvilable { get; set; } = true;
        public int categoryID { get; set; }

        [ForeignKey(nameof(categoryID))]
        public virtual Category category { get; set; }

        public int PhotoID { get; set; }

        [ForeignKey(nameof(PhotoID))]
        public virtual Photo Photo { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}

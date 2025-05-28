using RestrantApplication.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.Models.Order
{
    public class Order : BaseModel<int>
    {
        [Required]
        public OrderState orderState { get; set; }
        [Required]
        public OrderType orderType { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser ApplictionUser { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }
}


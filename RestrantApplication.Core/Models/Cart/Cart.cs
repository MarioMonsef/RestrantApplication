using RestrantApplication.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.Models.Cart
{
    public class Cart : BaseModel<int>
    {
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser ApplictionUser { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}

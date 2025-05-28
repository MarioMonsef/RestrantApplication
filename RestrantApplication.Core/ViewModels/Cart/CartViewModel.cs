using RestrantApplication.Core.Models.Cart;
using RestrantApplication.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Cart
{
    public record CartViewModel
    {
        public int ID { get; set; }

        public string UserID { get; set; }

        public ICollection<CartItemsViewModel> CartItems { get; set; } = new List<CartItemsViewModel>();
    }
}

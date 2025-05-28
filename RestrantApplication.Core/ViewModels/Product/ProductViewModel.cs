using RestrantApplication.Core.Models.Product;
using RestrantApplication.Core.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Product
{
    public record ProductViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Prise { get; set; }
        public decimal Stock { get; set; }
        public bool IsAvilable { get; set; }
        public virtual CategoryProductViewModel category { get; set; }
        public virtual PhotoProductViewModel Photo { get; set; }
    }
}

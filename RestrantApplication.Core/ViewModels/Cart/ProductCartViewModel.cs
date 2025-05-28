using RestrantApplication.Core.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Cart
{
    public record ProductcartViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Prise { get; set; }
        public decimal Stock { get; set; }
        public bool IsAvilable { get; set; }

        public virtual PhotoCartViewModel Photo { get; set; }
    }
}

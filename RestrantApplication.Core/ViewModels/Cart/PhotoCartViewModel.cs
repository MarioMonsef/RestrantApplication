using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Cart
{
    public record PhotoCartViewModel
    {
        public int ID { get; set; }
        public string PhotoName { get; set; }

    }
}

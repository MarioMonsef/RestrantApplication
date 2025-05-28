using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Order
{
    public record ApplicationUserOrderViewModel
    {
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
    }
}

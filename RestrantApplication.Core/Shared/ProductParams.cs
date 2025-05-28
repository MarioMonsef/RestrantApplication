using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.Shared
{
    public class ProductParams
    {
        public int? CategoryID { get; set; }
        public int PageNumber { get; set; } = 1;
        public string? Search { get; set; }
        public int PageSize { get; set; } = 9;


    }
}

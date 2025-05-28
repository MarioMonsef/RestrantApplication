using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.Models.Product
{
    public class Photo:BaseModel<int>
    {
        [Required]
        public string PhotoName { get; set; }

        public virtual Product Product { get; set; }
    }
}

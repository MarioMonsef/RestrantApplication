
using RestrantApplication.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.Models.Review
{
    public class Review : BaseModel<int>
    {
        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime ReviewDate { get; set; } = DateTime.Now;

        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser ApplictionUser { get; set; }

    }
}

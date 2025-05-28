using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.Models.Identity
{
    public class UserPicture:BaseModel<int>
    {
        [Required]
        public string PictureName { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}

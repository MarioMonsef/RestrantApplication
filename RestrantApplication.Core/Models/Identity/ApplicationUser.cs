using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Address { get; set; }
        public int PictureID { get; set; }
        [ForeignKey("PictureID")]
        public virtual UserPicture UserPicture { get; set; }
        public virtual ICollection<RestrantApplication.Core.Models.Order.Order> Orders { get; set; }
        public virtual RestrantApplication.Core.Models.Cart.Cart Cart { get; set; }
        public virtual ICollection<RestrantApplication.Core.Models.Review.Review> Reviews { get; set; }
    }
}

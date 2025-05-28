using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Identity
{
    public class UpdateRoleViewModel
    {
        public string ID { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}

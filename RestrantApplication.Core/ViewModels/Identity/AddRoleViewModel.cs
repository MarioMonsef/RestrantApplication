using System.ComponentModel.DataAnnotations;

namespace RestrantApplication.Core.ViewModels.Identity
{
    public record AddRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}

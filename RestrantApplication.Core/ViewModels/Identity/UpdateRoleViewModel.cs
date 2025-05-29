using System.ComponentModel.DataAnnotations;

namespace RestrantApplication.Core.ViewModels.Identity
{
    public record UpdateRoleViewModel
    {
        public string ID { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}

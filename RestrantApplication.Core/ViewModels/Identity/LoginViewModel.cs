using System.ComponentModel.DataAnnotations;

namespace RestrantApplication.Core.ViewModels.Identity
{
    public record LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public bool RemeberMe { get; set; } = false;

    }
}

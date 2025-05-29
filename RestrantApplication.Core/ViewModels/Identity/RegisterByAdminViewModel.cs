using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RestrantApplication.Core.ViewModels.Identity
{
    public record RegisterByAdminViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string PasswordHash { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.ImageUrl)]
        public IFormFile Picture { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}

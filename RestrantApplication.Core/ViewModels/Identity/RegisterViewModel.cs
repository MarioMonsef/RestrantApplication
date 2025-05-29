using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RestrantApplication.Core.ViewModels.Identity
{
    public record RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Length(11,11,ErrorMessage ="Phone Number Not Valid")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.ImageUrl)]
        public IFormFile Picture { get; set; }

    }
}

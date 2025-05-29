using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestrantApplication.Core.ViewModels.Product
{
    public record AddProductAndImageViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Prise { get; set; }
        [Required]
        public decimal Stock { get; set; }
        [Required]
        public bool IsAvilable { get; set; } = true;
        [Required]
        public int categoryID { get; set; }

        [Required]
        public IFormFile Image { get; set; }

    }
}

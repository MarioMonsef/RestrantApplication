using Microsoft.AspNetCore.Http;
using RestrantApplication.Core.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace RestrantApplication.Core.ViewModels.Product
{
    public class UpdateProductViewModel
    {
        public int ID { get; set; }
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
        public IFormFile? Image { get; set; }
        public int OldPhotoID { get; set; }
    }
}

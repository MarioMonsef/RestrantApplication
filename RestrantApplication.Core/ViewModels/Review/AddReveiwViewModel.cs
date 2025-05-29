using System.ComponentModel.DataAnnotations;

namespace RestrantApplication.Core.ViewModels.Review
{
    public record AddReveiwViewModel
    {
        [DataType(DataType.MultilineText)]
        [Required]
        public string Comment { get; set; }
    }
}

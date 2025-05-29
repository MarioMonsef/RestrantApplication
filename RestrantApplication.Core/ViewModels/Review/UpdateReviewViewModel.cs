using System.ComponentModel.DataAnnotations;

namespace RestrantApplication.Core.ViewModels.Review
{
    public record UpdateReviewViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Comment { get; set; }
        public string UserID { get; set; }
    }
}

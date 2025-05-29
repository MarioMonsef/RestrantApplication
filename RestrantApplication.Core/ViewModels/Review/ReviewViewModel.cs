namespace RestrantApplication.Core.ViewModels.Review
{
    public record ReviewViewModel
    {
        public int ID { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public virtual ApplicationUserReviewViewModel ApplictionUser { get; set; }
    }
}

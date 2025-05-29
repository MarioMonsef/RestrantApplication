namespace RestrantApplication.Core.ViewModels.Order
{
    public record ApplicationUserOrderViewModel
    {
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
    }
}

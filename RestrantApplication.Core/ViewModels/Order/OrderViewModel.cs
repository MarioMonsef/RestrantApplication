using RestrantApplication.Core.Models.Order;

namespace RestrantApplication.Core.ViewModels.Order
{
    public record OrderViewModel
    {
        public int ID { get; set; }
        public OrderState orderState { get; set; }
        public OrderType orderType { get; set; }

        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public virtual ApplicationUserOrderViewModel ApplictionUser { get; set; }
        public virtual List<OrderItemsViewModel> OrderItems { get; set; }
    }
}

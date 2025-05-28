using RestrantApplication.Core.Models.Order;

namespace RestrantApplication.EF.Services.Order__State_Design_Pattern_
{
    public class CompletedStateHandler : IOrderStateHandler
    {
        public bool StateChange(Order order, string? userId, string? role)
        {
            if (role == "Delivery Boy") {
                order.orderState = OrderState.DeliveryNow;
                return true;
            }
            return false;
        }
    }
}

using RestrantApplication.Core.Models.Order;

namespace RestrantApplication.EF.Services.Order__State_Design_Pattern_
{
    public class ProccessingStateHandler : IOrderStateHandler
    {
        public bool StateChange(Order order, string? userId, string? role)
        {
            if (role == "Chef") {
                order.orderState = OrderState.Completed;
                return true;
            }
            return false;
        }
    }
}

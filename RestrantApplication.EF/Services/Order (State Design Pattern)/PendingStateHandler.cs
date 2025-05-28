using RestrantApplication.Core.Models.Order;

namespace RestrantApplication.EF.Services.Order__State_Design_Pattern_
{
    public class PendingStateHandler : IOrderStateHandler
    {
        private readonly OrderState _target;

        public PendingStateHandler(OrderState target)
        {
            _target = target;
        }
        public bool StateChange(Order order, string? userId, string? role)
        {
            if (_target == OrderState.Processing && role == "Chef") { 
                order.orderState = OrderState.Processing;
                return true;
            }
            if (_target == OrderState.Cancelled && (role == "Chef" || order.UserID == userId)) { 
                order.orderState = OrderState.Cancelled;
                return true;
            }
            return false;
        }
    }
}

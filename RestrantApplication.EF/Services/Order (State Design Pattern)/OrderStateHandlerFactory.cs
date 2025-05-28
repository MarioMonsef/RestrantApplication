using RestrantApplication.Core.Models.Order;

namespace RestrantApplication.EF.Services.Order__State_Design_Pattern_
{
    public class OrderStateHandlerFactory
    {
        public static IOrderStateHandler? GetHandler(Order order,OrderState newState) {
            return order.orderState switch
            {
                OrderState.Pending => new PendingStateHandler(newState),
                OrderState.Processing when newState == OrderState.Completed => new ProccessingStateHandler(),
                OrderState.Completed when newState == OrderState.DeliveryNow => new CompletedStateHandler(),
                OrderState.DeliveryNow when newState == OrderState.DelivaryDone => new DeliveryNowStateHandler(),
                _=> null
            };
        }
    }
}

using RestrantApplication.Core.Models.Order;

namespace RestrantApplication.EF.Services.Order__State_Design_Pattern_
{
    public interface IOrderStateHandler
    {
        bool StateChange(Order order, string? userId, string? role);
    }
}

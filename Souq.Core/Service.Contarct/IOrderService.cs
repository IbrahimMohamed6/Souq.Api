

using Souq.Core.Entites.OrderAggregate;

namespace Souq.Core.Service.Contarct
{
    public interface IOrderService
    {
        Task<Order> CrateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, OrderAddress ShippingAddress);
        Task<IReadOnlyList<Order>> GetOrderForSpecificUser(string BuyerEmail);
        Task<Order> GetByIdOrderForSpecificUser(string BuyerEmail, int OrderId);
         Task<Order> UpdateOrderStatus(string paymentIntentId, bool flag);

    }
}

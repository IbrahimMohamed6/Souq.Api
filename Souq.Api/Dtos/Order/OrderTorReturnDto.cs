using Souq.Api.Dtos.Identity;
using Souq.Core.Entites.OrderAggregate;

namespace Souq.Api.Dtos.Order
{
    public class OrderTorReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public AddressIdentityDto ShippingAddress { get; set; }
        public int DeliveryMethodId { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; }
    }
}

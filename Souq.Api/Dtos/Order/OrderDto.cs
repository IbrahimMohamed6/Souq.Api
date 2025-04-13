using Souq.Api.Dtos.Identity;
using Souq.Core.Entites.OrderAggregate;

namespace Souq.Api.Dtos.Order
{
    public class OrderDto
    {
        public string basketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressIdentityDto shipToAddress { get; set; }
    }
}

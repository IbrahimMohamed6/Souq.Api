

using Souq.Core.Entites.Products;
using System.Net;

namespace Souq.Core.Entites.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {

        }
        public Order(string buyerEmauil, OrderAddress shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subtbtal, string paymentIntentId)
        {
            BuyerEmail = buyerEmauil;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subtbtal;
            PaymentIntentId = paymentIntentId;
        }



        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

        public OrderStatus Status { get; set; }
        public OrderAddress ShippingAddress { get; set; }
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }
        public decimal GetTotal()
        => SubTotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; }

    }
}

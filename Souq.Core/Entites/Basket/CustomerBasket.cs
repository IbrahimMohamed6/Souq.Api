namespace Souq.Core.Entites.Basket
{
    public class CustomerBasket
    {
        public string Id { get; set; }

        public List<BasketItem> Items { get; set; }=new();
        public CustomerBasket(string id)
        {
            Id=id;
        }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
    }
}

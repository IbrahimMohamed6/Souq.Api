using Souq.Core.Entites.Products;

namespace Souq.Api.Dtos.Basket
{
    public class CustomerBasketToReturnDto
    {
        public string Id { get; set; }

        public List<BasketItemToReturnDto> Items { get; set; }=new ();

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
    }
}

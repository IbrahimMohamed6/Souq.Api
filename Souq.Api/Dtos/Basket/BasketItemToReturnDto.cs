using System.ComponentModel.DataAnnotations;

namespace Souq.Api.Dtos.Basket
{
    public class BasketItemToReturnDto
    {
        public int Id { get; set; }
        
        public string productName { get; set; }
        public string PictureUrl { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        [Range(0.1, double.MaxValue, ErrorMessage = "Price Can Be Not Zero ")]

        public decimal Price { get; set; }
        [Range(1,int.MaxValue,ErrorMessage ="Quantity Must Have Grater Than 1")]
        public int Quantity { get; set; }
    }
}
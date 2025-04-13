namespace Souq.Core.Entites.Products
{
    public class Product:BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string PictureUrl { get; set; } = null!;

        public int BrandId { get; set; } // Foregin Key

        public int CategoryId { get; set; } //Foregin Key

        public Brand Brand { get; set; }//Navigational Property

        public Category Category { get; set; } //Navigational Property

    }
}

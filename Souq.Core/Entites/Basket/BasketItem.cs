﻿namespace Souq.Core.Entites.Basket
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string productName { get; set; }
        public string PictureUrl { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}

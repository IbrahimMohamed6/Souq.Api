using Souq.Core.Entites.OrderAggregate;
using Souq.Core.Entites.Products;
using System.Text.Json;

namespace Souq.Infrastructure.Data.DbContexts
{
    public static class DataSeedDbContext
    {
        public static async Task SeedAsync(StoreContext context)
        {
            #region Brands
            if (!context.Brands.Any())
            {
                var brandData = File.ReadAllText("../Souq.Infrastructure/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<Brand>>(brandData);

                if (brands is not null && brands.Count() > 0)
                {
                    foreach (var brand in brands)
                    {
                        context.Set<Brand>().Add(brand);
                    }
                }
                await context.SaveChangesAsync(); 
            }
            #endregion

            #region Category

            if (!context.Categories.Any())
            {
                var CategoryData = File.ReadAllText("../Souq.Infrastructure/DataSeed/Category.json");
                var category = JsonSerializer.Deserialize<List<Category>>(CategoryData);

                if (category is not null && category.Count() > 0)
                {
                    foreach (var cat in category)
                    {
                        context.Set<Category>().Add(cat);
                    }
                }
                await context.SaveChangesAsync(); 
            }


            #endregion

            #region Product

            if (!context.Products.Any())
            {
                var ProductData = File.ReadAllText("../Souq.Infrastructure/DataSeed/products.json");
                var Product = JsonSerializer.Deserialize<List<Product>>(ProductData);

                if (Product is not null && Product.Count() > 0)
                {
                    foreach (var Pro in Product)
                    {
                        context.Set<Product>().Add(Pro);
                    }
                }
                await context.SaveChangesAsync(); 
            }
            #endregion

            #region DeliveryMethod
            if (!context.DeliveyMethods.Any())
            {
                var DeliveryData = File.ReadAllText("../Souq.Infrastructure/DataSeed/delivery.json");
                var Delivery = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);

                if (Delivery is not null && Delivery.Count() > 0)
                {
                    foreach (var del in Delivery)
                    {
                        context.Set<DeliveryMethod>().Add(del);
                    }
                }
                await context.SaveChangesAsync();
            }
            #endregion
        }
    }
}

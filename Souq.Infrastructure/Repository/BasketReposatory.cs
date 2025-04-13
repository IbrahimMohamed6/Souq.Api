using Souq.Core.Entites.Basket;
using Souq.Core.RepositoryContract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Souq.Infrastructure.Repository
{
    public class BasketReposatory : IBasketReposatory
    {
        private readonly IDatabase _database;
        public BasketReposatory()
        {
            var options = new ConfigurationOptions
            {
                EndPoints = { "thorough-shepherd-25337.upstash.io:6379" }, // استبدل بعنوان Upstash
                Password = "AWL5AAIjcDEzYzUyMDQ3MzQwNTE0ODcwOTM4YTUyNDVjNGZkMGZjMHAxMA", // استبدل بكلمة المرور
                Ssl = true,
                AbortOnConnectFail = false
            };

            var multiplexer = ConnectionMultiplexer.Connect(options);
            _database = multiplexer.GetDatabase();
        }


        public async Task<CustomerBasket?> GetBasket(string basketId)
        {
            var basketData = await _database.StringGetAsync(basketId);
            return basketData.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basketData);
        }


        public async Task<CustomerBasket?> UpdateBasket(CustomerBasket basket)
        {
            if (string.IsNullOrEmpty(basket.Id))
            {
                Console.WriteLine("❌ Basket ID cannot be null or empty.");
                return null;
            }

            try
            {
                // التأكد من اتصال Redis
                var ping = await _database.PingAsync();
                Console.WriteLine($"✅ Redis Ping Time: {ping.TotalMilliseconds} ms");

                // تحويل البيانات إلى JSON
                var jsonBasket = JsonSerializer.Serialize(basket);
                Console.WriteLine($"Basket JSON: {jsonBasket}");

                // حفظ البيانات في Upstash Redis
                var updateBasket = await _database.StringSetAsync(basket.Id, jsonBasket, TimeSpan.FromDays(30));

                if (!updateBasket)
                {
                    Console.WriteLine("❌ Failed to update basket in Redis.");
                    return null;
                }

                Console.WriteLine("✅ Basket updated successfully!");

                return await GetBasket(basket.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error updating basket: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteBasket(string BasketId)
        {
            return await _database.KeyDeleteAsync(BasketId);

        }


    }
}

using System.Text.Json;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;
using StackExchange.Redis;

namespace Skinet.Infrastructure.Services
{
    public class CartService(IConnectionMultiplexer redis) : ICartService
    {
        private readonly IDatabase _database = redis.GetDatabase();

        public async Task<bool> DeleteCartAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<ShoppingCart?> GetCartAsync(string key)
        {
            RedisValue data = await _database.StringGetAsync(key);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data!);
        }

        public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
        {
            bool created = await _database.StringSetAsync(cart.Id,
                JsonSerializer.Serialize(cart), TimeSpan.FromDays(7));

            return created ? cart : await GetCartAsync(cart.Id);
        }
    }
}

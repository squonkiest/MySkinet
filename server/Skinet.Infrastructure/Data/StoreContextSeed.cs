using System.Text.Json;
using Skinet.Core.Entities;

namespace Skinet.Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.Products.Any())
            {
                string productsData = await File.ReadAllTextAsync("../Skinet.Infrastructure/Data/SeedData/products.json");
                List<Product>? products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products == null) return;

                context.AddRange(products);

                await context.SaveChangesAsync();
            }
        }
    }
}

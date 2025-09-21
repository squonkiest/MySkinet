using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;

namespace Skinet.Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _dbContext;

        public ProductRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            _dbContext.Products.Remove(product);
        }


        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _dbContext.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
        {
            IQueryable<Product> query = _dbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                query = query.Where(q => q.Brand == brand);
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(q => q.Type == type);
            }

            query = sort switch
            {
                "priceAsc" => query.OrderBy(x => x.Price),
                "priceDesc" => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.Name)
            };

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await _dbContext.Products.Select(x => x.Brand)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await _dbContext.Products.Select(x => x.Type)
                .Distinct()
                .ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return _dbContext.Products.Any(x => x.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void UpdateProduct(Product product)
        {
            _dbContext.Entry(product).State = EntityState.Modified;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;

namespace Skinet.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductRepository productRepository) : ControllerBase
    {
        private readonly IProductRepository _productRepository = productRepository;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            return Ok(await _productRepository.GetProductsAsync(brand, type, sort));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return Ok(await _productRepository.GetProductByIdAsync(id));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await _productRepository.GetBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await _productRepository.GetTypesAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            _productRepository.AddProduct(product);
            if (await _productRepository.SaveChangesAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }

            return BadRequest("Failed to create new product.");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !ProductExists(id))
            {
                return BadRequest("Cannot update this product");
            }

            _productRepository.UpdateProduct(product);
            if (await _productRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            Product? product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.DeleteProduct(product);

            if (await _productRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed to delete this product.");
        }

        private bool ProductExists(int id)
        {
            return _productRepository.ProductExists(id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Skinet.API.RequestHelpers;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;
using Skinet.Core.Specifications;

namespace Skinet.API.Controllers
{
    public class ProductsController(IGenericRepository<Product> productRepository) : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository = productRepository;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var specification = new ProductSpecification(specParams);
            return await CreatePagedResult(_productRepository, specification,
                specParams.PageIndex, specParams.PageSize); ;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return Ok(await _productRepository.GetByIdAsync(id));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await _productRepository.ListAsync(new BrandListSpecification()));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await _productRepository.ListAsync(new TypeListSpecification()));
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            _productRepository.Add(product);
            if (await _productRepository.SaveAllAsync())
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

            _productRepository.Update(product);
            if (await _productRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            Product? product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.Remove(product);

            if (await _productRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed to delete this product.");
        }

        private bool ProductExists(int id)
        {
            return _productRepository.Exists(id);
        }
    }
}

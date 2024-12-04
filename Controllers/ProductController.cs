using FluentValidation;
using LGC_CodeChallenge.Models;
using LGC_CodeChallenge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LGC_CodeChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: api/product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(product);
        }
        // POST: api/product
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            try
            {
                await _productService.AddProductAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }
        // PUT: api/product/{guid}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, Product product)
        {
            if (id != product.Id.ToString())
            {
                return BadRequest("Product ID in the URL does not match the Product ID in the body.");
            }
            try
            {
                var existingProduct = await _productService.GetProductAsync(id);
                if (existingProduct == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                await _productService.AddProductAsync(product); // Save replaces the existing entry
                return NoContent(); // 204 - No Content
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }
        // DELETE: api/product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var existingProduct = await _productService.GetProductAsync(id);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            await _productService.DeleteProductAsync(id);
            return NoContent(); // 204 - No Content
        }

    }
}

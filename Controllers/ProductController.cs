using FluentValidation;
using LGC_CodeChallenge.Interfaces;
using LGC_CodeChallenge.Models;
using LGC_CodeChallenge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LGC_CodeChallenge.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(product);
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetProductsList()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products); // Returns 200 OK with the list of products
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving products.", details = ex.Message });
            }
        }
        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            try
            {
                await _productService.AddProductAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id.ToString() }, product);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }
        // PUT: api/products/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, Product product)
        {
            if (id != product.Id)
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
        // DELETE: api/products/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
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

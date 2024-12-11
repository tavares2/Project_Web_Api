using AutoMapper;
using FluentValidation;
using LGC_Code_Challenge.Contracts;
using LGC_CodeChallenge.Interfaces;
using LGC_CodeChallenge.Models;
using LGC_CodeChallenge.Validators;
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
        private readonly IMapper _mapper;
        

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
            
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

            var productResponse = _mapper.Map<ProductResponse>(product);
            return Ok(productResponse);
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetProductsList()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products.Select(product => _mapper.Map<ProductResponse>(product))); // Returns 200 OK with the list of products
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving products.", details = ex.Message });
            }
        }
        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductRequest request)
        {
            // Ensure the DTO is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(request);

            product.Id = Guid.NewGuid();

            try
            {
                await _productService.AddProductAsync(product);
                var productResponse = _mapper.Map<ProductResponse>(product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productResponse);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            
        }
        // PUT: api/products/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, ProductRequest request)
        {
            // Ensure the DTO is valid
            if (!ModelState.IsValid)
            {
               return BadRequest(ModelState);
            }

            // Fetch the existing product
            var existingProduct = await _productService.GetProductAsync(id);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }


            // Attempt to update the product
            try
            {
                // Map changes from the DTO to the existing product
                _mapper.Map(request, existingProduct);

                await _productService.UpdateProductAsync(existingProduct);

                var productResponse = _mapper.Map<ProductResponse>(existingProduct);
                return CreatedAtAction(nameof(GetProduct), new { id = existingProduct.Id }, productResponse);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors); // Return validation errors if any
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

using Amazon.DynamoDBv2.DataModel;
using FluentValidation;
using LGC_Code_Challenge.Contracts;
using LGC_CodeChallenge.Data;
using LGC_CodeChallenge.Interfaces;
using LGC_CodeChallenge.Models;
using LGC_CodeChallenge.Repositories;
using LGC_CodeChallenge.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LGC_CodeChallenge.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository) 
        { 
            _productRepository = productRepository;
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddAsync(product);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            var product = await _productRepository.GetAsync(id);
            return product;
            
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products;
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }
    }
}


using Amazon.DynamoDBv2.DataModel;
using FluentValidation;
using LGC_CodeChallenge.Data;
using LGC_CodeChallenge.Models;
using LGC_CodeChallenge.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LGC_CodeChallenge.Services
{
    public class ProductService
    {
        
        private readonly AppDynamoDBContext _dynamoDbContext;
        private readonly ProductValidator _validator;
        private readonly DynamoDBTableManager _tableManager;

        public ProductService(AppDynamoDBContext dynamoDbContext, ProductValidator validator, DynamoDBTableManager tableManager)
        {
            _dynamoDbContext = dynamoDbContext;
            _validator = validator;
            _tableManager = tableManager;

            // Ensure the table exists during startup
            _tableManager.EnsureTableExistsAsync<Product>().Wait();
        }

        
        public async Task<Product> GetProductAsync(string id)
        {
            // Retrieve the product from DynamoDB by its ID
            var product = await _dynamoDbContext.LoadAsync<Product>(id);
            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            // Validate product using FluentValidation
            var validationResult = _validator.Validate(product);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Add the product to DynamoDB
            await _dynamoDbContext.SaveAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            // Validate product before updating
            var validationResult = _validator.Validate(product);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Update the product in DynamoDB
            await _dynamoDbContext.SaveAsync(product);
        }

        public async Task DeleteProductAsync(string id)
        {
            // Retrieve the product to ensure it exists
            var product = await _dynamoDbContext.LoadAsync<Product>(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            // Delete the product from DynamoDB
            await _dynamoDbContext.DeleteAsync(product);
        }
    }
}


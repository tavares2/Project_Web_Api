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
        
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ProductValidator _validator;
        private readonly DynamoDBTableManager _tableManager;

        public ProductService(IDynamoDBContext dynamoDbContext, ProductValidator validator, DynamoDBTableManager tableManager)
        {
            _dynamoDbContext = dynamoDbContext;
            _validator = validator;
            _tableManager = tableManager;
        }

        public async Task SaveModelAsync(Product product)
        {
            await _tableManager.EnsureTableExistsAsync<Product>();

            // Validate the model
            var validationResult = await _validator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Save to DynamoDB
            await _dynamoDbContext.SaveAsync(product);
        }
    }
}


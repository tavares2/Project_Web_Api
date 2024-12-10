using FluentValidation;
using LGC_CodeChallenge.Data;
using LGC_CodeChallenge.Interfaces;
using LGC_CodeChallenge.Models;
using LGC_CodeChallenge.Validators;

namespace LGC_CodeChallenge.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        
        private readonly IValidator<Product> _validator;

        public ProductRepository(AppDynamoDBContext dynamoDbContext, IValidator<Product> validator) : base(dynamoDbContext)
        {
            _validator = validator;
        }
        
        public new async Task AddAsync(Product product)
        {
            // Validate product using FluentValidation
            //var validationResult = _validator.Validate(product);
            //if (!validationResult.IsValid)
            //{
            //    throw new ValidationException(validationResult.Errors);
            //}
            //
            // Add the product to DynamoDB
            await base.AddAsync(product);
        }

        public new async Task UpdateAsync(Product product)
        {
            // Validate product before updating
            var validationResult = _validator.Validate(product);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Update the product in DynamoDB
            await base.UpdateAsync(product);
        }
    }
}

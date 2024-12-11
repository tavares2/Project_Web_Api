using FluentValidation;
using LGC_CodeChallenge.Data;
using LGC_CodeChallenge.Interfaces;
using LGC_CodeChallenge.Models;
using LGC_CodeChallenge.Validators;

namespace LGC_CodeChallenge.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        

        public ProductRepository(AppDynamoDBContext dynamoDbContext) : base(dynamoDbContext)
        {
        }
        
        public new async Task AddAsync(Product product)
        {
            
            await base.AddAsync(product);
        }

        public new async Task UpdateAsync(Product product)
        {
            

            // Update the product in DynamoDB
            await base.UpdateAsync(product);
        }
    }
}

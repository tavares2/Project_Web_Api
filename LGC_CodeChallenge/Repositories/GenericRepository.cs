using LGC_CodeChallenge.Data;
using LGC_CodeChallenge.Interfaces;
using LGC_CodeChallenge.Models;

namespace LGC_CodeChallenge.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntityBase
    {
        private readonly AppDynamoDBContext _dynamoDbContext;

        public GenericRepository(AppDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task<T> GetAsync(Guid id)
        {
            // Retrieve the product from DynamoDB by its ID
            var entity = await _dynamoDbContext.LoadAsync<T>(id);
            return entity;
        }

        public async Task<List<T>> GetAllAsync()
        {
            var entities = await _dynamoDbContext.LoadAllAsync<T>();
            return entities;
        }

        public async Task AddAsync(T entity)
        {
             
            // Add the product to DynamoDB
            await _dynamoDbContext.SaveAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            //Update the product to DynamoDB
            await _dynamoDbContext.SaveAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            // Retrieve the product to ensure it exists
            var entity = await _dynamoDbContext.LoadAsync<T>(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }

            // Delete the product from DynamoDB
            await _dynamoDbContext.DeleteAsync(entity);
        }
    }
}

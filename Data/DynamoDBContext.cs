using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;


namespace LGC_CodeChallenge.Data
{
    public class DynamoDBContext : IDynamoDBContext
    {
        private readonly DynamoDBContext  _context;
        public DynamoDBContext(IAmazonDynamoDB dynamoDBClient) 
        { 
            _context = new DynamoDBContext(dynamoDBClient);
        }

        // Example method to access the wrapped context
        public async Task SaveAsync<T>(T entity) where T : class
        {
            await _context.SaveAsync(entity);
        }

    }
}

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;


namespace LGC_CodeChallenge.Data
{
    public class AppDynamoDBContext: DynamoDBContext
    {
        public AppDynamoDBContext(IAmazonDynamoDB dynamoDBClient)
        : base(dynamoDBClient)
        {
        }


        public async Task SaveAsync<T>(T entity) where T : class
        {
            
            await base.SaveAsync(entity);
        }

        public async Task<T> LoadAsync<T>(object hashKey) where T : class
        {
            
            return await base.LoadAsync<T>(hashKey);
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            
            await base.DeleteAsync(entity);
        }

    }
}

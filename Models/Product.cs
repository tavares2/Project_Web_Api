using Amazon.DynamoDBv2.DataModel;
using LGC_CodeChallenge.Interfaces;
namespace LGC_CodeChallenge.Models

{
    [DynamoDBTable("ProductTable")]
    public class Product : IEntityBase
    {
        [DynamoDBHashKey("id")]
        public Guid Id { get; set; }

        [DynamoDBProperty]
        public string Name { get; set; }

        [DynamoDBProperty]
        public decimal Price { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }

        [DynamoDBProperty]
        public int Stock {  get; set; }
    }
}
